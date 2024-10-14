using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.WPF.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECR.WPF.ViewModels {
    public partial class ContactViewModel : ObservableObject {
        public int Id { get; set; } = -1;
        public bool IsNew => Id == -1;

        [ObservableProperty]
        bool isDefault = false;

        [ObservableProperty]
        ContactType contactType = ContactType.Mobile;

        [ObservableProperty]
        string value = "";

        [RelayCommand]
        void CopyToClipboard() {
            Clipboard.SetText(Value.Replace(" ", "").Trim());
            MessageBox.Show("Copied to clipboard.", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    public abstract partial class BaseAgencyFormViewModel : ObservableValidator, ICloseableObject {
        protected BaseAgencyFormViewModel(IDBContextFactory contextFactory) {
            this.contextFactory = contextFactory;
        }

        public event EventHandler? OnClose;

        const string REQUIRED_FIELD_STRING = "*Requred";
        [RelayCommand]
        public void Close() {
            this.OnClose?.Invoke(this, EventArgs.Empty);
        }

        public ObservableCollection<ContactViewModel> Contacts { get; set; } = [];

        public ContactType[] ContactTypeChoices { get; } = [ContactType.Mobile, ContactType.Telephone, ContactType.Email, ContactType.Messenger];

        bool CanAddContact => !string.IsNullOrWhiteSpace(ContactValue) && Contacts.Count < 10;

        [RelayCommand(CanExecute = nameof(CanAddContact))]
        void AddContact() {

            var contact = new ContactViewModel() {
                ContactType = ContactType,
                Value = ContactValue.TrimmedAndNullWhenEmpty()!
            };

            Contacts.Add(contact);

            ContactValue = string.Empty;
        }

        [RelayCommand]
        void RemoveContact(ContactViewModel contact) {
            if (MessageBox.Show("Are you sure you want to remove this contact information?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {

                Contacts.Remove(contact);

                if (contact.IsDefault) {

                    var newDefaultContact = Contacts.FirstOrDefault();

                    if (newDefaultContact is not null)
                        newDefaultContact.IsDefault = true;

                }
            }
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddContactCommand))]
        string contactValue = string.Empty;

        [ObservableProperty]
        ContactType contactType = ContactType.Mobile;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string name = null!;

        [ObservableProperty]
        private string? contactDetails = null;

        [ObservableProperty]
        private string? address = null;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemoveLogoCommand))]
        private ImageSource? logo = null;

        bool HasLogo => Logo is not null;

        protected IDBContextFactory contextFactory { get; init; }

        protected void InvokeSaveEvent(object p) {
            OnSaveSuccessful?.Invoke(this, p);
        }

        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        async Task Save() {
            await SaveAgency();
        }

        [RelayCommand(CanExecute = nameof(HasLogo))]
        void RemoveLogo() {
            if (Logo is not null &&
                MessageBox.Show("Are you sure you want to remove logo", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Logo = null;
        }

        /// <summary>
        /// the actual implementation of save
        /// </summary>
        /// <returns></returns>
        protected virtual async Task SaveAgency() {
            await Task.CompletedTask;
            Close();
        }
        protected virtual bool ResetAgency() {
            return MessageBox.Show("Are you sure you want to reset changes?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        [RelayCommand]
        void Reset() {
            ResetAgency();
        }

        //public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        void PickImage() {
            Microsoft.Win32.OpenFileDialog dlg = new() {
                Filter = "Image Files | *.jpg;*.jpeg;*.png;"
            };

            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true) {
                // Open document 
                string filename = dlg.FileName;
                //extension = Path.GetExtension(dlg.FileName);
                Logo = new BitmapImage(new Uri(filename));
            }
        }
    }

    public partial class Form_Add_Agency_ViewModel(IDBContextFactory contextFactory) : BaseAgencyFormViewModel(contextFactory) {
        protected override bool ResetAgency() {
            if (base.ResetAgency()) {

                Name = ContactDetails = Address = string.Empty;
                Logo = null;
                return true;
            }

            return false;
        }

        protected override async Task SaveAgency() {
            ValidateAllProperties();

            if (HasErrors) {
                MessageBox.Show("Fill out required fields first!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = contextFactory.CreateDbContext()) {

                var agency = new Agency() {
                    Name = Name.TrimmedAndNullWhenEmpty()!,
                    Address = Address.TrimmedAndNullWhenEmpty(),
                    Logo = Logo.ToByteArray(),
                    ContactDetails = Contacts.Select(c =>
                    new ContactDetail() {
                        Type = c.ContactType,
                        Value = c.Value,
                        IsDefault = c.IsDefault
                    })
                    .ToList()
                };

                var result = context.Agencies.Add(agency);
                await context.SaveChangesAsync();

                InvokeSaveEvent(result.Entity);
            }

            await base.SaveAgency();
        }
    }
    public partial class Form_Edit_Agency_ViewModel(IDBContextFactory contextFactory) : BaseAgencyFormViewModel(contextFactory) {
        private Agency agency = null!;

        public Agency AgencyToEdit {
            get { return agency; }
            set {
                agency = value;
                Name = agency.Name;
                Address = agency.Address.TrimmedAndNullWhenEmpty();
                Logo = agency.Logo?.ToImageSource();

                foreach (var c in agency.ContactDetails.Select(c => new ContactViewModel() { Id = c.Id, ContactType = c.Type, Value = c.Value, IsDefault = c.IsDefault }).ToList())
                    Contacts.Add(c);
            }
        }

        protected override bool ResetAgency() {
            if (base.ResetAgency()) {
                Name = agency.Name;
                //ContactDetails = agency.ContactInfo;
                Address = agency.Address;
                Logo = agency.Logo.ToImageSource();
                return true;
            }
            return false;
        }

        protected override async Task SaveAgency() {
            ValidateAllProperties();

            if (HasErrors) {
                MessageBox.Show("Fill out required fields first!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try {
                using var context = contextFactory.CreateDbContext();

                var agencyToEdit = await context.Agencies
                    .Include(a => a.ContactDetails)
                    .FirstOrDefaultAsync(a => a.Id == agency.Id);

                if (agencyToEdit is null) return;

                agencyToEdit.Name = Name.TrimmedAndNullWhenEmpty()!;
                agencyToEdit.Address = Address.TrimmedAndNullWhenEmpty();
                agencyToEdit.Logo = Logo.ToByteArray();

                ///delete
                foreach (var contact in agencyToEdit.ContactDetails.ToArray()) {
                    if (!Contacts.Any(c => c.Id == contact.Id)) {
                        agencyToEdit.ContactDetails.Remove(contact);
                    }
                }

                foreach (var vm in Contacts) {
                    if (vm.Id == -1)
                        agencyToEdit.ContactDetails.Add(new ContactDetail() { Type = vm.ContactType, Value = vm.Value, IsDefault = vm.IsDefault });
                    else {
                        var edit = agencyToEdit.ContactDetails.FirstOrDefault(c => c.Id == vm.Id)!;
                        edit.Value = vm.Value;
                        edit.IsDefault = vm.IsDefault;
                    }
                }

                await context.SaveChangesAsync();

                InvokeSaveEvent(agencyToEdit);

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await base.SaveAgency();
        }
    }
}
