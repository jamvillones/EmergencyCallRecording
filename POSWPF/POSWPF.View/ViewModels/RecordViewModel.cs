using CommunityToolkit.Mvvm.ComponentModel;
using ECR.Domain.Models;
using ECR.WPF.Utilities;
using System.Windows.Media;

namespace ECR.View.ViewModels {
    public sealed partial class Record_Item_ViewModel : ObservableObject {
        private Record record = null!;

        public int Id => record.Id;

        public Record Record {
            get => record;

            set {
                record = value;
                CallerDetails = record.Call?.ToString()!;
                CallType = record.CallType!;
                Summary = record.Summary!;
                Agency = new Agency_Item_ViewModel() { Agency = record.Agency! };
                Level = record.PriorityLevel;
                PlaceOfIncident = record.IncidentLocation;
                DateTimeOfReport = record.DateTimeOfReport;
            }
        }

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

        public event EventHandler<bool>? OnSelectionChanged;

    }
}
