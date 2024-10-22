using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.View.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;

namespace ECR.WPF.ViewModels.Tabs {
    public interface ISearchableObject {
        Task Search(string keyword);
    }
    sealed partial class RecordTabs : ObservableObject {
        public RecordTabs(IViewModelFactory viewModelFactory, IModalViewer modalViewer) {
            ViewModelFactory = viewModelFactory;
            ModalViewer = modalViewer;
            callsTab = ViewModelFactory.Get<Records_CallsSection>();
            agencyTab = ViewModelFactory.Get<Records_AgenciesSection>();

            callsTab.OnEdit += OnEdit;
            agencyTab.OnEdit += OnEdit;
            currentTab = callsTab;
        }

        private void OnEdit(object? sender, object e) {
            var regForm = (ICloseableObject)e;
            ModalViewer.SetModal(regForm);
        }

        readonly Records_CallsSection callsTab = null!;
        readonly Records_AgenciesSection agencyTab = null!;

        [ObservableProperty]
        IRegistrationOpener currentTab = null!;

        //[ObservableProperty]
        //private ObservableObject? _openedForm = null;

        public IViewModelFactory ViewModelFactory { get; }
        public IModalViewer ModalViewer { get; }

        [RelayCommand]
        async Task Search(string keyword) {
            if (string.IsNullOrWhiteSpace(keyword))
                return;

            if (CurrentTab is ISearchableObject searchable) {
                await searchable.Search(keyword.Trim());
                return;
            }

            throw new Exception("Current tab is not of type ISearcheable");
        }

        [RelayCommand]
        async Task Refresh() {
            if (CurrentTab is ISearchableObject searchable) {
                await searchable.Search(string.Empty);
                return;
            }
        }

        [RelayCommand]
        private void OpenRegistrationForm() {
            var regForm = CurrentTab.GetRegistrationForm();
            ModalViewer.SetModal(regForm);
            //SubscribeToCloseEvent(regForm);
            //OpenedForm = regForm as ObservableObject;
        }

        //void SubscribeToCloseEvent(ICloseableObject closeable) {
        //    closeable.OnClose += Closeable_OnClose;
        //}
        //private void Closeable_OnClose(object? sender, EventArgs e) {
        //    OpenedForm = null;
        //}

        [RelayCommand]
        void SwitchToCalls() {
            CurrentTab = callsTab;
        }

        [RelayCommand]
        void SwitchToAgencies() {
            CurrentTab = agencyTab;
        }
    }
    interface IRegistrationOpener {
        ICloseableObject GetRegistrationForm();
        ICloseableObject GetEditForm(int id);
        event EventHandler<object> OnEdit;
    }

    sealed partial class Records_CallsSection : ObservableObject, IRegistrationOpener, ISearchableObject {

        public IDBContextFactory ContextFactory { get; }
        public IViewModelFactory ViewModelFactory { get; }

        public Records_CallsSection(IDBContextFactory contextFactory, IViewModelFactory viewModelFactory) {
            ContextFactory = contextFactory;
            ViewModelFactory = viewModelFactory;
            _ = InitializeData();
        }

        private string? _keyword;
        async Task InitializeData(string? keyword = null) {
            try {
                using var context = ContextFactory.CreateDbContext();

                var records = await context.Records.AsNoTracking().AsQueryable()
                    .Include(r => r.Agency)
                    .FilterRecord(keyword!)
                    .OrderByDescending(r => r.DateTimeOfReport)
                    .ToListAsync();

                if (records.Count != 0) Records.Clear();

                foreach (var rec in records)
                    Records.Add(CreateRecordViewModel(rec));
            }
            catch (Exception) { }
        }

        public ObservableCollection<Record_Item_ViewModel> Records { get; } = [];

        public event EventHandler<object>? OnEdit;

        [RelayCommand]
        private void OpenEditForm(int id) {
            var regForm = GetEditForm(id);
            OnEdit?.Invoke(this, regForm);
        }

        Record_Item_ViewModel CreateRecordViewModel(Record record) {
            var recordVm = ViewModelFactory.Get<Record_Item_ViewModel>();
            recordVm.Record = record;
            recordVm.OnSelectionChanged += Record_OnSelectionChanged;
            return recordVm;
        }

        [RelayCommand]
        async Task RemoveItem(Record_Item_ViewModel vm) {
            if (MessageBox.Show("Are you sure you want to remove this record?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return;

            try {
                using var context = ContextFactory.CreateDbContext();

                var toRemove = await context.Records.FirstOrDefaultAsync(r => r.Id == vm.Id);
                if (toRemove is null)
                    return;

                context.Records.Remove(toRemove);
                await context.SaveChangesAsync();

                Records.Remove(vm);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// We have this method for unsubscribing 'onselectionchanged' event when record is removed
        /// </summary>
        void ClearRecords() {
            foreach (var item in Records) item.OnSelectionChanged -= Record_OnSelectionChanged;
            Records.Clear();
        }

        /// <summary>
        /// we inform the ui that the selection has changed, thus calculating if the the main checkbox should be checked or not based on condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Record_OnSelectionChanged(object? sender, bool e) {
            OnPropertyChanged(nameof(AllItemsAreChecked));
            OnPropertyChanged(nameof(ItemsSelected));
        }

        public ICloseableObject GetRegistrationForm() {
            var newRecordForm = ViewModelFactory.Get<Form_Add_Record_ViewModel>();
            newRecordForm.OnSaveSuccessful += NewRecordForm_OnSaveSuccessful;
            return newRecordForm;
        }

        /// <summary>
        /// handle the addtition of new record 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecordForm_OnSaveSuccessful(object? sender, object e) {
            if (e is Record rec) {
                Records.Insert(0, CreateRecordViewModel(rec));
            }
        }

        public ICloseableObject GetEditForm(int id) {
            using var context = ContextFactory.CreateDbContext();
            var recordToEdit = context.Records
                .Include(r => r.Agency)
                .Include(r => r.Audios)
                .FirstOrDefault(x => x.Id == id);

            var newRecordForm = ViewModelFactory.Get<Form_Edit_Record_ViewModel>();
            newRecordForm.Record = recordToEdit!;
            return newRecordForm;
        }

        public async Task Search(string keyword) {
            _keyword = keyword;
            await InitializeData(_keyword);
        }

        public int ItemsSelected => Records.Where(x => x.IsChecked).Count();

        public bool AllItemsAreChecked {
            get => Records.Any() && Records.All(x => x.IsChecked);

            set {
                foreach (var item in Records) item.IsChecked = value;
                OnPropertyChanged();
            }
        }

        public int TotalItems => Records.Count;
    }

    sealed partial class Records_AgenciesSection : ObservableObject, IRegistrationOpener, ISearchableObject {
        private IDBContextFactory ContextFactory { get; }
        private IViewModelFactory ViewModelFactory { get; }

        public event EventHandler<object>? OnEdit;

        public Records_AgenciesSection(IDBContextFactory contextFactory, IViewModelFactory viewModelFactory) {
            this.ContextFactory = contextFactory;
            this.ViewModelFactory = viewModelFactory;

            _ = LoadDataAsync();
        }

        [RelayCommand]
        async Task RemoveAgency(Agency_Item_ViewModel vm) {

            if (MessageBox.Show("Are you sure you want to remove this agency? This action cannot be undone.", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;

            try {
                using (var context = ContextFactory.CreateDbContext()) {
                    var agency = await context.Agencies.FirstOrDefaultAsync(x => x.Id == vm.Id);
                    context.Agencies.Remove(agency!);
                    await context.SaveChangesAsync();
                }

                Items.Remove(vm);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Delete failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void OpenEditForm(int id) {

            var regForm = GetEditForm(id);
            if (regForm is Form_Edit_Agency_ViewModel form) {
                form.OnSaveSuccessful += Form_OnSaveSuccessful;
            }
            OnEdit?.Invoke(this, regForm);

        }
        /// <summary>
        /// handling successful edit of agency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_OnSaveSuccessful(object? sender, object e) {
            if (e is Agency a) {
                var toUpdate = Items.FirstOrDefault(i => i.Id == a.Id)!;
                toUpdate.Agency = a;
            }
        }

        async Task LoadDataAsync(string? keyword = null) {
            using var context = ContextFactory.CreateDbContext();

            var agencies = await context.Agencies
                .AsQueryable()
                .AsNoTracking()
                .Include(a => a.ContactDetails)
                .FilterAgency(keyword!)
                .OrderBy(x => x.Name.First)
                    .ThenBy(x => x.Name.Last)
                .ToListAsync();

            if (agencies.Count != 0)
                Items.Clear();

            foreach (var a in agencies)
                AddNewItem(CreateAgencyViewModel(a));
        }

        private Agency_Item_ViewModel CreateAgencyViewModel(Agency agency) {
            var vm = ViewModelFactory.Get<Agency_Item_ViewModel>();
            vm.Agency = agency;
            return vm;
        }

        void AddNewItem(Agency_Item_ViewModel item) {
            Items.Add(item);
            item.OnSelectionChanged += Item_OnSelectionChanged; ;
            OnPropertyChanged(nameof(TotalItems));
        }

        private void Item_OnSelectionChanged(object? sender, bool e) {
            OnPropertyChanged(nameof(AllItemsAreChecked));
            OnPropertyChanged(nameof(ItemsSelected));
        }

        public ObservableCollection<Agency_Item_ViewModel> Items { get; } = [];

        public ICloseableObject GetRegistrationForm() {
            var newRecordForm = ViewModelFactory.Get<Form_Add_Agency_ViewModel>();
            newRecordForm.OnSaveSuccessful += NewRecordForm_OnSaveSuccessful;
            return newRecordForm;
        }

        private void NewRecordForm_OnSaveSuccessful(object? sender, object e) {
            var agency = (Agency)e;

            AddNewItem(CreateAgencyViewModel(agency));
        }

        public ICloseableObject GetEditForm(int id) {
            using var context = ContextFactory.CreateDbContext();
            var agency = context.Agencies.Include(a => a.ContactDetails).FirstOrDefault(a => a.Id == id);
            var newRecordForm = ViewModelFactory.Get<Form_Edit_Agency_ViewModel>();
            newRecordForm.AgencyToEdit = agency!;

            //newRecordForm.OnSaveSuccessful += NewRecordForm_OnSaveSuccessful;
            return newRecordForm;
        }
        private string? _keyword = null;
        public async Task Search(string keyword) {
            _keyword = keyword;
            await LoadDataAsync(_keyword);
        }

        public int TotalItems => Items.Count;
        public int ItemsSelected => Items.Where(x => x.IsChecked).Count();

        public bool AllItemsAreChecked {
            get => Items.Count == 0 ? false : Items.All(x => x.IsChecked);
            set {
                foreach (var item in Items) item.IsChecked = value;
                OnPropertyChanged();
            }
        }
    }
}
