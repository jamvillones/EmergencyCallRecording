using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ECR.View.ViewModels.Tabs {

    sealed partial class RecordTabs : ObservableObject {
        public RecordTabs() {
            for (int i = 0; i < 10; i++)
                AddNewItem(new RecordViewModel());

            OnPropertyChanged(nameof(TotalItems));
        }

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

        public ObservableCollection<RecordViewModel> Records { get; } = [];

        [ObservableProperty]
        private ObservableObject? _openedForm = null;

        [RelayCommand]
        private void OpenRegistrationForm() {
            var regForm = new AddRecordForm_ViewModel();
            SubscribeToCloseEvent(regForm);
            OpenedForm = regForm;
        }

        void SubscribeToCloseEvent(ICloseableObject closeable) {
            closeable.OnClose += Closeable_OnClose;
        }

        private void Closeable_OnClose(object? sender, EventArgs e) {
            OpenedForm = null;
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
}
