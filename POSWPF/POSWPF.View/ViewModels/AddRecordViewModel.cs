using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows;

namespace ECR.WPF.ViewModels {
    public interface ICloseableObject {
        event EventHandler OnClose;
        void Close();
    }

    public enum Level { One, Two, Three, Four, Five, Six }
    public abstract partial class BaseRecordForm_ViewModel : ObservableValidator, ICloseableObject {

        protected BaseRecordForm_ViewModel(IDBContextFactory dbFactory) {
            DbFactory = dbFactory;
        }

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

        protected void InvokeSaveEvent(object p) {
            OnSaveSuccessful?.Invoke(this, p);
        }

        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        void AddAudioFile() {
            Microsoft.Win32.OpenFileDialog dlg = new() {
                Filter = "Audio Files | *.mp3;*.ogg;*.wav;"
            };

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) {

                if (Audios.Any(a => a.FilePath == dlg.FileName)) {
                    MessageBox.Show("File Already on the list", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
                var newAudioVm = new AudioViewModel() {
                    Name = dlg.SafeFileName,
                    DateTimeRecorded = File.GetCreationTime(dlg.FileName),
                    Duration = Sound.SoundInfo.GetSoundLength(dlg.FileName),
                    FilePath = dlg.FileName
                };

                Audios.Add(newAudioVm);
            }

        }

        private const string REQUIRED_FIELD_STRING = "*Required";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string _callerName = null!;

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
        private string? _callType;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _title;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string _details = null!;

        public ObservableCollection<AudioViewModel> Audios { get; private set; } = [];
        protected IDBContextFactory DbFactory { get; init; }
    }
    public partial class AddRecordForm_ViewModel : BaseRecordForm_ViewModel {
        public AddRecordForm_ViewModel(IDBContextFactory dbFactory) : base(dbFactory) {
        }

        public override async Task<bool> Save() {
            if (!await base.Save()) {
                MessageBox.Show("Fill out necessary fields first!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using var context = DbFactory.CreateDbContext();

            var record = new Record() {
                Call = new Caller() { Name = CallerName, Address = CallAddresss, ContactDetails = CallContactDetails },
                Audios = Audios.Select(a => new Audio() { FilePath = a.FilePath, Name = a.Name, DateRecorded = a.DateTimeRecorded }).ToList(),
                Summary = Title,
                Details = Details
            };


            await context.SaveChangesAsync();
            return true;
        }

        protected override void Reset() {
            CallerName = CallAddresss = CallContactDetails = Severity = EndorseTo = Details = Title = CallType = string.Empty;
        }
    }
    public partial class EditRecordForm_ViewModel : BaseRecordForm_ViewModel {
        public EditRecordForm_ViewModel(IDBContextFactory dbFactory) : base(dbFactory) {
        }

        protected override void Reset() {
        }
    }

    public partial class AudioViewModel : ObservableObject {
        [ObservableProperty]
        string name = string.Empty;

        [ObservableProperty]
        int duration = 0;

        [ObservableProperty]
        DateTime dateTimeRecorded = DateTime.Now;

        [ObservableProperty]
        bool isChecked = false;

        [ObservableProperty]
        string filePath = null!;

        [ObservableProperty]
        int number = 1;

        [RelayCommand]
        void PlayAudio() {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo() { FileName = FilePath, UseShellExecute = true });
        }

    }
}
