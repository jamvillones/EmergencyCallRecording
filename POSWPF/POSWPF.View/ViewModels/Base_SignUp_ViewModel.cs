
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.WPF.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECR.WPF.ViewModels {
    interface IPasswordHandler {
        string GetPassword { get; }
        bool ValidatePassword();

        void Reset();

        bool IsPasswordValid { get; }
    }

    partial class Register_Login_PasswordHandler : ObservableValidator, IPasswordHandler {
        public string GetPassword => Password;

        public bool IsPasswordValid => Password == ConfirmPassword;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string password = null!;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string confirmPassword = null!;

        public bool ValidatePassword() {
            ValidateAllProperties();
            return !HasErrors;
        }

        public void Reset() {
            Password = ConfirmPassword = string.Empty;
        }
    }

    partial class Edit_Login_PasswordHandler : ObservableValidator, IPasswordHandler {
        public string GetPassword => throw new NotImplementedException();

        public bool IsPasswordValid => throw new NotImplementedException();

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string oldPassword = null!;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string newPassword = null!;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string confirmNewPassword = null!;

        public bool ValidatePassword() {
            throw new NotImplementedException();
        }

        public void Reset() {
            throw new NotImplementedException();
        }
    }

    abstract partial class Base_SignUp_ViewModel(IDBContextFactory dBContextFactory, IPasswordHandler passwordHandler) : ObservableValidator, ICloseableObject {
        public IDBContextFactory DBContextFactory { get; } = dBContextFactory;
        public IPasswordHandler PasswordHandler { get; } = passwordHandler;

        [ObservableProperty]
        ImageSource? photo = null;

        [RelayCommand]
        void PickPhoto() {
            Microsoft.Win32.OpenFileDialog dlg = new() {
                Filter = "Image Files | *.jpg;*.jpeg;*.png;"
            };

            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) {
                // Open document 
                string filename = dlg.FileName;
                //extension = Path.GetExtension(dlg.FileName);
                Photo = new BitmapImage(new Uri(filename));
            }
        }

        [RelayCommand]
        void RemovePhoto() {
            Photo = null;
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string username = null!;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string firstName = null!;

        [ObservableProperty]
        string middleName = null!;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "*Required!")]
        string lastName = null!;

        [ObservableProperty]
        string extensionName = null!;

        [ObservableProperty]
        string position = null!;

        public event EventHandler? OnClose;

        [RelayCommand]
        void Save() {
            SaveLogin();
        }

        [RelayCommand]
        void Reset() {
            DiscardLogin();
        }

        protected abstract Task SaveLogin();
        protected abstract Task DiscardLogin();

        [RelayCommand]
        public void Close() {
            OnClose?.Invoke(this, EventArgs.Empty);
        }
    }

    class SignUp_Form_ViewModel(IDBContextFactory dBContextFactory, [FromKeyedServices(FormSaveType.Register)] IPasswordHandler passwordHandler) : Base_SignUp_ViewModel(dBContextFactory, passwordHandler) {
        protected override async Task DiscardLogin() {
            await Task.CompletedTask;
            Photo = null;
            Username = FirstName = MiddleName = LastName = ExtensionName = Position = string.Empty;
            PasswordHandler.Reset();
        }

        protected override async Task SaveLogin() {
            ValidateAllProperties();


            if (HasErrors) {
                MessageBox.Show("Fill out required fields first!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!PasswordHandler.ValidatePassword()) {
                MessageBox.Show("Password Cannot Be Empty", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!PasswordHandler.IsPasswordValid) {
                MessageBox.Show("Password and Confirm Password do not match", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try {
                using var context = DBContextFactory.CreateDbContext();
                var newLogin = new Login() {
                    Photo = Photo.ToByteArray(),
                    Username = Username,
                    Name = new Name() {
                        First = FirstName.TrimmedAndNullWhenEmpty()!,
                        Middle = MiddleName.TrimmedAndNullWhenEmpty(),
                        Last = LastName.TrimmedAndNullWhenEmpty()!,
                        Extension = ExtensionName.TrimmedAndNullWhenEmpty()
                    },
                    Position = Position.TrimmedAndNullWhenEmpty(),
                    Password = PasswordHandler.GetPassword
                };

                await context.Logins.AddAsync(newLogin);
                await context.SaveChangesAsync();
                MessageBox.Show("Login Saved!", "Login Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (DbUpdateException) {
                MessageBox.Show("Username already taken!", "Change Username!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception) {

            }

        }
    }

    class EditLogin_Form_ViewModel(IDBContextFactory dBContextFactory, [FromKeyedServices(FormSaveType.Edit)] IPasswordHandler passwordHandler) : Base_SignUp_ViewModel(dBContextFactory, passwordHandler) {

        private Login _login = null!;

        public Login Login {
            get { return _login; }
            set {
                _login = value;

                Photo = _login.Photo.ToImageSource();
                Username = _login.Username;
                FirstName = _login.Name.First;
                MiddleName = _login.Name.Middle!;
                LastName = _login.Name.Last!;
                ExtensionName = _login.Name.Extension!;

                Position = _login.Position!;
            }
        }

        protected override Task DiscardLogin() {
            throw new NotImplementedException();
        }

        protected override Task SaveLogin() {
            throw new NotImplementedException();
        }
    }


}
