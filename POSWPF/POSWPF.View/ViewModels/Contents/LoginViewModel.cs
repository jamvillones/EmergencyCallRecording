using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.View.Utilities;
using ECR.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ECR.View.ViewModels.Contents {

    public enum LoginStatus { Pending, Okay, Failed, Disconnected }
    sealed partial class LoginViewModel(IDBContextFactory dBContextFactory, IViewModelFactory viewModelFactory) : ObservableObject {
        public IDBContextFactory DBContextFactory { get; } = dBContextFactory;
        public IViewModelFactory ViewModelFactory { get; } = viewModelFactory;

        public event EventHandler? OnLoginSuccessful;

        [ObservableProperty]
        LoginStatus loginStatus = LoginStatus.Pending;

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

            if (await TryLoginAsync(Username!, Password!)) {
                OnLoginSuccessful?.Invoke(this, EventArgs.Empty);
                return;
            }

            IsLoading = false;
        }

        async Task<bool> TryLoginAsync(string username, string password) {
            LoginStatus = LoginStatus.Pending;

            try {
                using var context = DBContextFactory.CreateDbContext();
                var login = await context.Logins.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
                await Task.Delay(1000);
                LoginStatus = login is null ? LoginStatus.Failed : LoginStatus.Okay;

            }
            catch {
                LoginStatus = LoginStatus.Disconnected;
                return false;
            }

            return LoginStatus != LoginStatus.Failed;
        }
    }
}
