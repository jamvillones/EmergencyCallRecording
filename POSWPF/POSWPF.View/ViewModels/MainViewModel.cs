using CommunityToolkit.Mvvm.ComponentModel;
using ECR.View.Utilities;
using ECR.View.ViewModels.Contents;

namespace ECR.View.ViewModels {
    sealed partial class MainViewModel : ObservableObject {
        public MainViewModel(IViewModelFactory viewModelFactory) {
            ViewModelFactory = viewModelFactory;

            CurrentPage = viewModelFactory.Get<LoginViewModel>();

            if (_currentPage is LoginViewModel login) {
                login.OnLoginSuccessful += Login_OnLoginSuccessful;
            }
        }

        [ObservableProperty]
        ObservableObject _currentPage;


        public IViewModelFactory ViewModelFactory { get; }

        private void Login_OnLoginSuccessful(object? sender, EventArgs e) {
            CurrentPage = ViewModelFactory.Get<MainContentViewModel>();
            if (CurrentPage is ILogOffable l) {
                l.OnLogOff += L_OnLogOff;
            }
        }

        private void L_OnLogOff(object? sender, EventArgs e) {
            CurrentPage = ViewModelFactory.Get<LoginViewModel>();

            if (CurrentPage is LoginViewModel login)
                login.OnLoginSuccessful += Login_OnLoginSuccessful;
        }
    }
}
