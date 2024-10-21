using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.View.ViewModels.Tabs;
using ECR.WPF.Utilities;
using ECR.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ECR.View.ViewModels.Contents {
    public interface ILogOffable {
        void LogOff();
        event EventHandler OnLogOff;
    }

    sealed partial class MainContentViewModel : ObservableObject, ILogOffable {
        public MainContentViewModel(INotificationHandler notificationHandler, IViewModelFactory vmFactory, ILoginHandler loginHandler, IModalViewer modalViewer) {
            NotificationHandler = notificationHandler;
            VmFactory = vmFactory;
            LoginHandler = loginHandler;
            ModalViewer = modalViewer;
            recordsTab = VmFactory.Get<RecordTabs>();
            CurrentTab = recordsTab;

            CurrentLoginPhoto = LoginHandler.Login!.Photo?.ToImageSource()!;
            CurrentLoginName = LoginHandler.Login!.Name;
            CurrentLoginPosition = LoginHandler.Login!.Position!;

        }
        public INotificationHandler NotificationHandler { get; } = null!;
        public IViewModelFactory VmFactory { get; } = null!;
        public ILoginHandler LoginHandler { get; }
        public IModalViewer ModalViewer { get; }

        [ObservableProperty]
        private ObservableObject _currentTab = null!;

        private readonly RecordTabs recordsTab = null!;

        public event EventHandler? OnLogOff;

        [RelayCommand]
        public void LogOff() {
            OnLogOff?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        void ShowCurrentUser() {
            var editLoginForm = VmFactory.Get<EditLogin_Form_ViewModel>();
            editLoginForm.Login = LoginHandler.Login!;
            ModalViewer.SetModal(editLoginForm);
        }

        [ObservableProperty]
        ImageSource currentLoginPhoto = null!;
        [ObservableProperty]
        Name currentLoginName = null!;
        [ObservableProperty]
        string currentLoginPosition = null!;
    }
}
