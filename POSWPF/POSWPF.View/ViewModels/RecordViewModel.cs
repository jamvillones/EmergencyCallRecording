using CommunityToolkit.Mvvm.ComponentModel;
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
        public int Id { get; set; } = -1;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        string? contactDetails = "";

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
