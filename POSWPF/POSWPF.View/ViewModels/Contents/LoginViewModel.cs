using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.WPF.Utilities;
using ECR.WPF.ViewModels;

namespace ECR.View.ViewModels.Contents {

    sealed partial class LoginViewModel : ObservableObject {
        public IDBContextFactory DBContextFactory { get; }
        public IViewModelFactory ViewModelFactory { get; }
        public ILoginHandler LoginHandler { get; }

        public LoginViewModel(IDBContextFactory dBContextFactory, IViewModelFactory viewModelFactory, ILoginHandler loginHandler) {
            DBContextFactory = dBContextFactory;
            ViewModelFactory = viewModelFactory;
            LoginHandler = loginHandler;

            var settings = ECR.WPF.Properties.Settings.Default;

            //if (!settings.IsValidated) 
            if (true) {
                var AppValidation = ViewModelFactory.Get<AuthenticationForm_ViewModel>();
                AppValidation.OnClose += AppValidation_OnClose;
                ModalObject = AppValidation;
            }
        }

        private void AppValidation_OnClose(object? sender, EventArgs e) {
            ModalObject = null;
        }

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
        ObservableValidator? modalObject = null;



        [RelayCommand]
        void OpenSignupForm() {
            ModalObject = ViewModelFactory.Get<SignUp_Form_ViewModel>();
            if (ModalObject is ICloseableObject closeable) {
                closeable.OnClose += Closeable_OnClose;
            }
        }

        private void Closeable_OnClose(object? sender, EventArgs e) {
            ModalObject = null;
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
