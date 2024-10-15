using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.ViewModels;
using ECR.WPF.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ECR.WPF.ViewModels {
    public interface ICloseableObject {
        event EventHandler OnClose;
        void Close();
    }

    public abstract partial class BaseRecordForm_ViewModel : ObservableValidator, ICloseableObject {
        protected BaseRecordForm_ViewModel(IDBContextFactory dbFactory) {
            DbFactory = dbFactory;
            Audios.CollectionChanged += Audios_CollectionChanged;

            //_ = InitializeAgencyList();
        }

        private void Audios_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged(nameof(AudiosIsEmpty));
            OnPropertyChanged(nameof(AudiosNotEmpty));
        }

        protected async Task InitializeAgencyList(Agency selected = null) {
            try {
                using var context = DbFactory.CreateDbContext();

                var agencies = await context.Agencies.Include(a => a.ContactDetails).AsNoTracking().ToListAsync();

                foreach (var agency in agencies) {
                    Agencies.Add(new Agency_Item_ViewModel() {
                        Agency = agency
                    });
                }

                SelectedAgency = Agencies.FirstOrDefault(x => x.Id == selected?.Id) ?? Agencies.First();
            }
            catch (Exception) { }
        }

        public ObservableCollection<Agency_Item_ViewModel> Agencies { get; } = [];

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

            if (HasErrors) {
                foreach (var e in GetErrors())
                    Debug.WriteLine(e.ToString());

                return false;
            }

            await Task.CompletedTask;
            return true;
        }

        protected void InvokeSaveEvent(object p) {
            OnSaveSuccessful?.Invoke(this, p);
        }

        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        void RemoveAudio(AudioViewModel audio) {
            Audios.Remove(audio);
        }

        [RelayCommand(CanExecute = nameof(AudiosNotEmpty))]
        void RemoveAllAudios() {
            Audios.Clear();
        }


        [RelayCommand]
        void AddAudioFile() {
            Microsoft.Win32.OpenFileDialog dlg = new() {
                Filter = "Audio Files | *.mp3;*.ogg;*.wav;",
                Multiselect = true
            };

            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) {

                foreach (var path in dlg.FileNames) {

                    if (Audios.Any(a => a.FilePath == path))
                        continue;

                    var newAudioVm = new AudioViewModel() {
                        Name = Path.GetFileName(path),
                        DateTimeRecorded = File.GetCreationTime(path),
                        //Duration = Sound.SoundInfo.GetSoundLength(path),
                        FilePath = path
                    };
                    Audios.Add(newAudioVm);
                }
            }
        }

        #region Binding Properties
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
        private PriorityLevel _level = PriorityLevel.One;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string _incidentLocation = null!;

        [ObservableProperty]
        Agency_Item_ViewModel selectedAgency = null!;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string? _callType;

        [ObservableProperty]

        private string? _summary = null;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string _details = null!;

        public ObservableCollection<AudioViewModel> Audios { get; } = [];
        public bool AudiosIsEmpty => Audios.Count == 0;
        public bool AudiosNotEmpty => Audios.Count > 0;
        #endregion

        protected IDBContextFactory DbFactory { get; init; }
    }

    public partial class Form_Add_Record_ViewModel : BaseRecordForm_ViewModel {
        public Form_Add_Record_ViewModel(IDBContextFactory dbFactory) : base(dbFactory) {
            _ = InitializeAgencyList();
        }

        public override async Task<bool> Save() {
            if (!await base.Save()) {
                MessageBox.Show("Fill out necessary fields first!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using var context = DbFactory.CreateDbContext();

            var record = new Record() {
                Call = new Caller() {
                    Name = CallerName,
                    ContactDetail = CallContactDetails.TrimmedAndNullWhenEmpty()!,
                    Address = CallAddresss.TrimmedAndNullWhenEmpty()!
                },

                CallType = CallType.TrimmedAndNullWhenEmpty(),
                Agency = context.Agencies.FirstOrDefault(a => a.Id == SelectedAgency.Agency.Id),
                IncidentLocation = IncidentLocation.TrimmedAndNullWhenEmpty()!,
                PriorityLevel = Level,
                Summary = Summary.TrimmedAndNullWhenEmpty(),
                Details = Details.TrimmedAndNullWhenEmpty()!,

                Audios = Audios.Select(a => new Audio() {
                    FilePath = a.FilePath,
                    Name = a.Name,
                    DateRecorded = a.DateTimeRecorded
                }).ToList(),
            };

            await context.Records.AddAsync(record);
            await context.SaveChangesAsync();

            InvokeSaveEvent(record);
            Close();
            return true;
        }

        protected override void Reset() {
            if (MessageBox.Show("Are you sure you want to reset?", "These changes cannot be made", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return;

            CallerName = CallAddresss = CallContactDetails = IncidentLocation = Summary = CallType = string.Empty;
        }
    }

    public partial class Form_Edit_Record_ViewModel(IDBContextFactory dbFactory) : BaseRecordForm_ViewModel(dbFactory) {

        private Record record = null!;
        public Record Record {
            get { return record; }
            set {
                record = value;
                CallerName = record.Call.Name;
                CallContactDetails = record.Call.ContactDetail;
                CallAddresss = record.Call.Address;

                Level = record.PriorityLevel;
                CallType = record.CallType;
                IncidentLocation = record.IncidentLocation;
                Summary = record.Summary;
                Details = record.Details;

                foreach (var audio in record.Audios)
                    Audios.Add(new AudioViewModel() { Audio = audio });

                _ = InitializeAgencyList(record.Agency!);
            }
        }

        public override async Task<bool> Save() {
            if (!await base.Save()) {
                MessageBox.Show("Fill out necessary fields first!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try {
                using var context = DbFactory.CreateDbContext();

                var toSave = await context.Records
                    .Include(r => r.Agency)
                    .Include(r => r.Audios)
                    .FirstOrDefaultAsync(x => x.Id == record.Id);

                if (toSave is null) return false;

                toSave.Call.Name = CallerName;
                toSave.Call.ContactDetail = CallContactDetails.TrimmedAndNullWhenEmpty()!;
                toSave.Call.Address = CallAddresss.TrimmedAndNullWhenEmpty();

                toSave.PriorityLevel = Level;
                toSave.CallType = CallType;
                toSave.IncidentLocation = IncidentLocation.TrimmedAndNullWhenEmpty()!;
                toSave.Summary = Summary.TrimmedAndNullWhenEmpty()!;
                toSave.Details = Details.TrimmedAndNullWhenEmpty()!;
                toSave.Agency = context.Agencies.FirstOrDefault(a => a.Id == SelectedAgency.Agency.Id);

                ///delete
                foreach (var audio in toSave.Audios.ToArray()) {
                    if (!Audios.Any(c => c.Id == audio.Id)) {
                        toSave.Audios.Remove(audio);
                    }
                }

                foreach (var audio in Audios.Where(a => a.Id == -1))
                    toSave.Audios.Add(new Audio { DateRecorded = audio.DateTimeRecorded, FilePath = audio.FilePath, Name = audio.Name });

                await context.SaveChangesAsync();

                InvokeSaveEvent(toSave);
                Close();

            }
            catch (Exception) {

            }

            return true;

        }



        protected override void Reset() {
        }
    }

    public partial class AudioViewModel : ObservableObject {
        private Audio _audio = null!;
        public Audio Audio {
            get { return _audio; }
            set {
                _audio = value;
                Id = _audio.Id;
                Name = _audio.Name;
                DateTimeRecorded = _audio.DateRecorded;
                FilePath = _audio.FilePath;
            }
        }

        public int Id { get; private set; } = -1;


        [ObservableProperty]
        string name = string.Empty;

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
