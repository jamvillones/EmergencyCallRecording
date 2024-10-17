using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.WPF.Utilities;
using ECR.WPF.ViewModels;
using System.Windows;
using System.Windows.Media;

namespace ECR.View.ViewModels {
    public sealed partial class Record_Item_ViewModel : ObservableObject {
        public Record_Item_ViewModel(IViewModelFactory viewModelFactory) {
            ViewModelFactory = viewModelFactory;
        }
        private Record record = null!;

        public int Id => record.Id;

        public Record Record {
            get => record;

            set {
                record = value;
                CallerDetails = record.Call?.ToString()!;
                CallType = record.CallType!;
                Summary = record.Summary!;
                Agency = ViewModelFactory.Get<Agency_Item_ViewModel>();
                Agency.Agency = record.Agency!;

                Level = record.PriorityLevel;
                PlaceOfIncident = record.IncidentLocation;
                DateTimeOfReport = record.DateTimeOfReport;
            }
        }

        public IViewModelFactory ViewModelFactory { get; }

        [ObservableProperty]
        string _callerDetails = null!;

        [ObservableProperty]
        string _callType = null!;

        [ObservableProperty]
        string _summary = null!;

        [ObservableProperty]
        PriorityLevel _level = PriorityLevel.One;

        [ObservableProperty]
        Agency_Item_ViewModel _agency = null!;

        [ObservableProperty]
        string _placeOfIncident = "Poblacion, Kalibo, Aklan";

        [ObservableProperty]
        bool _isChecked = false;

        [ObservableProperty]
        DateTime _dateTimeOfReport;

        partial void OnIsCheckedChanged(bool value) {
            OnSelectionChanged?.Invoke(this, value);
        }

        public event EventHandler<bool>? OnSelectionChanged;
    }

    public sealed partial class Agency_Item_ViewModel : ObservableObject {
        public Agency_Item_ViewModel(NotificationHandler notificationHandler) {
            NotificationHandler = notificationHandler;
        }

        private Agency _agency = null!;
        public Agency Agency {
            get => _agency;
            set {
                _agency = value;
                Id = _agency.Id;
                Name = _agency.Name;
                DefaultContactDetail = _agency.ContactDetails.FirstOrDefault(x => x.IsDefault);
                Address = _agency.Address;
                Logo = _agency.Logo.ToImageSource();
                DefaultContactDetail = _agency.DefaultContact;
            }
        }

        public int Id { get; set; } = -1;
        public NotificationHandler NotificationHandler { get; }

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        ContactDetail? defaultContactDetail = null;

        [ObservableProperty]
        string? address = "";

        [ObservableProperty]
        bool _isChecked = false;

        [ObservableProperty]
        ImageSource? logo = null;

        partial void OnIsCheckedChanged(bool value) {
            OnSelectionChanged?.Invoke(this, value);
        }

        [RelayCommand]
        async Task CopyDefaultContact() {
            try {

                Clipboard.SetText(DefaultContactDetail?.Value.Replace(" ", ""));
                NotificationHandler.RaiseNotification("Copied to Clipboard", DefaultContactDetail?.Value!);

                await Task.Delay(1000);
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event EventHandler<bool>? OnSelectionChanged;

    }
}
