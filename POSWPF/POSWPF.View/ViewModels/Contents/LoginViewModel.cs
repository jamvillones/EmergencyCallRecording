using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ECR.View.ViewModels.Contents {
    sealed partial class LoginViewModel : ObservableObject {

        public event EventHandler? OnLoginSuccessful;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string? _username;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        string? _password;

        [ObservableProperty]
        bool _isLoading = false;

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

        static async Task<bool> TryLoginAsync(string username, string password) {
            await Task.Delay(1000);
            return true;

        }
    }
}
