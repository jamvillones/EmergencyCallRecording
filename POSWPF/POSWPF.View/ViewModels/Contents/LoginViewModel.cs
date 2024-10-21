using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.WPF.Utilities;
using ECR.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECR.View.ViewModels.Contents {

    sealed partial class LoginViewModel(IDBContextFactory dBContextFactory, IViewModelFactory viewModelFactory, ILoginHandler loginHandler) : ObservableObject {
        public IDBContextFactory DBContextFactory { get; } = dBContextFactory;
        public IViewModelFactory ViewModelFactory { get; } = viewModelFactory;
        public ILoginHandler LoginHandler { get; } = loginHandler;

        public event EventHandler? OnLoginSuccessful;

        [ObservableProperty]
        LoginStatusType loginStatus = LoginStatusType.Pending;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string? _username;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string? _password;

        [ObservableProperty]
        bool _isLoading = false;

        [ObservableProperty]
        ObservableValidator? signupForm = null;

        [RelayCommand]
        void OpenSignupForm() {
            SignupForm = ViewModelFactory.Get<SignUp_Form_ViewModel>();
            if (SignupForm is ICloseableObject closeable) {
                closeable.OnClose += Closeable_OnClose;
            }
        }

        private void Closeable_OnClose(object? sender, EventArgs e) {
            SignupForm = null;
        }

        bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);



        [RelayCommand(CanExecute = nameof(CanLogin))]
        async Task Login() {
            IsLoading = true;
            LoginStatus = LoginStatusType.Pending;

            if (await LoginHandler.TryLoginAsync(Username!, Password!)) {
                OnLoginSuccessful?.Invoke(this, EventArgs.Empty);
            }

            LoginStatus = LoginHandler.LoginStatus;
            IsLoading = false;
        }
    }
}
