using CommunityToolkit.Mvvm.ComponentModel;
using ECR.View.Utilities;
using ECR.View.ViewModels.Contents;

namespace ECR.View.ViewModels {
    sealed partial class MainViewModel : ObservableObject {
        public MainViewModel(IViewModelFactory viewModelFactory) {
            ViewModelFactory = viewModelFactory;

            if (_currentPage is LoginViewModel login) {
                login.OnLoginSuccessful += Login_OnLoginSuccessful;
            }
        }

        [ObservableProperty]
        ObservableObject _currentPage = new LoginViewModel();


        public IViewModelFactory ViewModelFactory { get; }

        private void Login_OnLoginSuccessful(object? sender, EventArgs e) {
            CurrentPage = ViewModelFactory.Get<MainContentViewModel>();
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
