using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace ECR.View.ViewModels {
    public interface ICloseableObject {
        event EventHandler OnClose;
        void Close();
    }

    public enum Level { One, Two, Three, Four, Five, Six }
    abstract partial class BaseRecordForm_ViewModel : ObservableValidator, ICloseableObject {
        public event EventHandler? OnClose;
        [RelayCommand]
        public void Close() {
            this.OnClose?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        protected abstract void Reset();

        [RelayCommand]
        public virtual async Task<bool> Save() {
            ValidateAllProperties();

            if (HasErrors) return false;

            await Task.CompletedTask;
            return true;
        }

        private const string REQUIRED_FIELD_STRING = "*Required";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _callerName = null!;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _callContactDetails = null!;

        [ObservableProperty]
        private string? _callAddresss;

        [ObservableProperty]
        private Level _level = Level.One;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _severity;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _endorseTo;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _title;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _details;
    }
    partial class AddRecordForm_ViewModel : BaseRecordForm_ViewModel {
        public override async Task<bool> Save() {
            if (!await base.Save()) {
                MessageBox.Show("Fill out necessary fields first!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        protected override void Reset() {
            CallerName = CallAddresss = CallContactDetails = Severity = EndorseTo = Details = Title = string.Empty;
        }
    }
    partial class EditRecordForm_ViewModel : BaseRecordForm_ViewModel {
        protected override void Reset() {
        }
    }
}
