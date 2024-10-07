using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.WPF.Utilities;
using ECR.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace ECR.View.ViewModels.Tabs {

    sealed partial class RecordTabs : ObservableObject {
        public RecordTabs() {

            currentTab = callsTab;
        }

        readonly CallsTab callsTab = new();
        readonly AgencyTab agencyTab = new(new DbContextFactory());

        [ObservableProperty]
        IRegistrationOpener currentTab = null!;

        [ObservableProperty]
        private ObservableObject? _openedForm = null;

        [RelayCommand]
        private void OpenRegistrationForm() {
            var regForm = CurrentTab.GetRegistrationForm();
            SubscribeToCloseEvent(regForm);
            OpenedForm = regForm as ObservableObject;
        }
        void SubscribeToCloseEvent(ICloseableObject closeable) {
            closeable.OnClose += Closeable_OnClose;
        }
        private void Closeable_OnClose(object? sender, EventArgs e) {
            OpenedForm = null;
        }

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
    }

    sealed partial class CallsTab : ObservableObject, IRegistrationOpener {
        public CallsTab() {
            for (int i = 0; i < 10; i++)
                AddNewItem(new RecordViewModel());

            OnPropertyChanged(nameof(TotalItems));
        }
        public ObservableCollection<RecordViewModel> Records { get; } = [];

        void AddNewItem(RecordViewModel record) {
            Records.Add(record);
            record.OnSelectionChanged += Record_OnSelectionChanged;
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
            var newRecordForm = new AddRecordForm_ViewModel(new DbContextFactory());
            newRecordForm.OnSaveSuccessful += NewRecordForm_OnSaveSuccessful;
            return newRecordForm;
        }

        /// <summary>
        /// handle the addtition of new record 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRecordForm_OnSaveSuccessful(object? sender, object e) {
            RecordViewModel record = new RecordViewModel();
            AddNewItem(record);
        }

        public int ItemsSelected => Records.Where(x => x.IsChecked).Count();

        public bool AllItemsAreChecked {
            get => Records.All(x => x.IsChecked);
            set {
                foreach (var item in Records) item.IsChecked = value;
                OnPropertyChanged();
            }
        }

        public int TotalItems => Records.Count;
    }

    sealed partial class AgencyTab : ObservableObject, IRegistrationOpener {
        private readonly IDBContextFactory contextFactory;

        public AgencyTab(IDBContextFactory contextFactory) {
            this.contextFactory = contextFactory;
            _ = LoadDataAsync();
        }

        async Task LoadDataAsync() {
            using (var context = contextFactory.CreateDbContext()) {
                var agencies = await context.Agency.ToListAsync();

                foreach (var a in agencies)
                    AddNewItem(new AgencyViewModel() { Name = a.Name, Address = a.Address, ContactDetails = a.ContactInfo, Logo = a.Logo?.ToImageSource() });
            }
        }

        void AddNewItem(AgencyViewModel item) {
            Items.Add(item);
            item.OnSelectionChanged += Item_OnSelectionChanged; ;
            OnPropertyChanged(nameof(TotalItems));
        }

        private void Item_OnSelectionChanged(object? sender, bool e) {
            OnPropertyChanged(nameof(AllItemsAreChecked));
            OnPropertyChanged(nameof(ItemsSelected));
        }

        public ObservableCollection<AgencyViewModel> Items { get; } = [];

        public ICloseableObject GetRegistrationForm() {
            var newRecordForm = new AddAgencyFormViewModel(new DbContextFactory());
            newRecordForm.OnSaveSuccessful += NewRecordForm_OnSaveSuccessful;
            return newRecordForm;
        }

        private void NewRecordForm_OnSaveSuccessful(object? sender, object e) {
            var agency = (Agency)e;

            AddNewItem(new AgencyViewModel() { Name = agency.Name, Address = agency.Address, ContactDetails = agency.ContactInfo, Logo = agency.Logo?.ToImageSource() });
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
