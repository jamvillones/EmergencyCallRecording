using CommunityToolkit.Mvvm.ComponentModel;
using ECR.Domain.Models;
using ECR.WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ECR.View.ViewModels {
    public sealed partial class RecordViewModel : ObservableObject {

        [ObservableProperty]
        string _callerDetails = "Juan Dela Cruz - 0999 999 9999";
        [ObservableProperty]
        string _title = "Lorem ipsum dolor sit amet";
        [ObservableProperty]
        string _level = "VI";
        [ObservableProperty]
        string _severity = "Catastrophic";
        [ObservableProperty]
        string _endorseTo = "BFP Kalibo Subfirestation";

        [ObservableProperty]
        string _placeOfIncident = "Poblacion, Kalibo, Aklan";

        [ObservableProperty]
        bool _isChecked = false;

        partial void OnIsCheckedChanged(bool value) {
            OnSelectionChanged?.Invoke(this, value);
        }

        public event EventHandler<bool>? OnSelectionChanged;

    }

    public sealed partial class AgencyViewModel : ObservableObject {
        public void SetAgency(Agency agency) {
            Id = agency.Id;
            Name = agency.Name;
            DefaultContactDetail = agency.ContactDetails.FirstOrDefault(x => x.IsDefault);
            Address = agency.Address;
            Logo = agency.Logo.ToImageSource();
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
