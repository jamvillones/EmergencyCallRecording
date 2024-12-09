using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace ECR.WPF.ViewModels {
    partial class AuthenticationForm_ViewModel : ObservableValidator, ICloseableObject {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthenticateCommand))]
        [NotifyDataErrorInfo]
        [Required]
        private string username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthenticateCommand))]
        [NotifyDataErrorInfo]
        [Required]
        private string password = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AuthenticateCommand))]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage ="This field is required!")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address!")]
        private string receivingEmail = string.Empty;

        private bool CanAuthenticate => !HasErrors;
        [RelayCommand(CanExecute = nameof(CanAuthenticate))]
        async Task Authenticate() {

            var client = new HttpClient() {
                BaseAddress = new Uri("http://192.168.1.63:8080/")
            };

            var data = new {
                Username = Username.Trim(),
                Password = Password.Trim(),
                To = ReceivingEmail.Trim()
            };

            var payload = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("Authenticate", payload);

            if (response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();

                receivedOTP = content.Trim('"');
                OtpRecieved = true;

            }
            else {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                    MessageBox.Show("Wrong Credentials", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Connection not made", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string receivedOTP = string.Empty;

        [ObservableProperty]
        bool otpRecieved = false;

        [ObservableProperty]
        private string otp = string.Empty;

        public event EventHandler? OnClose;

        [RelayCommand]
        void Validate() {
            if (receivedOTP == Otp.Trim()) {
                MessageBox.Show("App Successfully Validated!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var settings = Properties.Settings.Default;
                settings.IsValidated = true;
                settings.Save();
                Close();
            }
        }

        public void Close() {
            OnClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
