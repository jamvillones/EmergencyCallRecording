using CommunityToolkit.Mvvm.ComponentModel;
using ECR.View.ViewModels.Contents;

namespace ECR.View.ViewModels {
    sealed partial class MainViewModel : ObservableObject {

        [ObservableProperty]
        ObservableObject _currentPage = new LoginViewModel();

        public MainViewModel() {
            if (_currentPage is LoginViewModel login) {
                login.OnLoginSuccessful += Login_OnLoginSuccessful;
            }
        }

        private void Login_OnLoginSuccessful(object? sender, EventArgs e) {
            CurrentPage = new MainContentViewModel();
            if (CurrentPage is ILogOffable l) {
                l.OnLogOff += L_OnLogOff;
            }
        }

        private void L_OnLogOff(object? sender, EventArgs e) {
            CurrentPage = new LoginViewModel();

            if (CurrentPage is LoginViewModel login)
                login.OnLoginSuccessful += Login_OnLoginSuccessful;

        }
    }
}
