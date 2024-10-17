using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.View.Utilities;
using ECR.View.ViewModels.Tabs;
using ECR.WPF.ViewModels;
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
        public MainContentViewModel(NotificationHandler notificationHandler, IViewModelFactory vmFactory) {
            NotificationHandler = notificationHandler;
            VmFactory = vmFactory;

            recordsTab = VmFactory.Get<RecordTabs>();

            CurrentTab = recordsTab;

        }

        [ObservableProperty]
        private ObservableObject _currentTab = null!;

        private readonly RecordTabs recordsTab = null!;

        public NotificationHandler NotificationHandler { get; } = null!;
        public IViewModelFactory VmFactory { get; } = null!;

        public event EventHandler? OnLogOff;

        [RelayCommand]
        public void LogOff() {
            OnLogOff?.Invoke(this, EventArgs.Empty);
        }
    }
}
