using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.View.ViewModels.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.View.ViewModels.Contents {
    public interface ILogOffable {
        void LogOff();
        event EventHandler OnLogOff;
    }
    sealed partial class MainContentViewModel : ObservableObject, ILogOffable {
        [ObservableProperty]
        private ObservableObject _currentTab;

        private RecordTabs recordsTab = new();

        public MainContentViewModel() {
            CurrentTab = recordsTab;
        }

        public event EventHandler OnLogOff;

        [RelayCommand]
        public void LogOff() {
            OnLogOff?.Invoke(this, EventArgs.Empty);
        }
    }
}
