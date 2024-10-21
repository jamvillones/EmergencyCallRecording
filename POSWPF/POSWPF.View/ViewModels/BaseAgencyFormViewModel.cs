using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.Utilities;
using ECR.WPF.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECR.WPF.ViewModels {
    public enum FormSaveType { Register, Edit }
    public partial class Contact_Item_ViewModel : ObservableObject {
        public Contact_Item_ViewModel(INotificationHandler notificationHandler) {
            NotificationHandler = notificationHandler;
        }
        public INotificationHandler NotificationHandler { get; }

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
            try {
                string toCopy = Value.Replace(" ", "").Trim();
                Clipboard.SetText(toCopy);
                NotificationHandler.RaiseNotification("Copied to Clipboard", "Contact Details: " + toCopy);
            }
            catch (Exception) { }
        }
    }
    public abstract partial class Form_BaseAgency_ViewModel(IDBContextFactory contextFactory, IViewModelFactory viewModelFactory, INotificationHandler notificationHandler) : ObservableValidator, ICloseableObject {
        protected IDBContextFactory ContextFactory { get; } = contextFactory;
        protected IViewModelFactory ViewModelFactory { get; } = viewModelFactory;
        protected INotificationHandler NotificationHandler { get; } = notificationHandler;

        public event EventHandler? OnClose;
        public abstract FormSaveType SaveType { get; }

        const string REQUIRED_FIELD_STRING = "*Requred";

        [RelayCommand]
        public void Close() {
            this.OnClose?.Invoke(this, EventArgs.Empty);
        }

        public ObservableCollection<Contact_Item_ViewModel> Contacts { get; set; } = [];

        public ContactType[] ContactTypeChoices { get; } = [ContactType.Mobile, ContactType.Telephone, ContactType.Email, ContactType.Messenger];

        bool CanAddContact => !string.IsNullOrWhiteSpace(ContactValue) && Contacts.Count < 10;

        [RelayCommand(CanExecute = nameof(CanAddContact))]
        void AddContact() {
            var contact = ViewModelFactory.Get<Contact_Item_ViewModel>();

            contact.ContactType = ContactType;
            contact.Value = ContactValue.TrimmedAndNullWhenEmpty()!;
            contact.IsDefault = Contacts.Count == 0;

            Contacts.Add(contact);

            ContactValue = string.Empty;
        }

        [RelayCommand]
        void RemoveContact(Contact_Item_ViewModel contact) {
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
        private string firstName = null!;

        [ObservableProperty]
        private string? middleName = null;
        [ObservableProperty]
        private string? lastName = null;
        [ObservableProperty]
        private string? extensionName = null;

        [ObservableProperty]
        private string? contactDetails = null;

        [ObservableProperty]
        private string? address = null;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemoveLogoCommand))]
        private ImageSource? logo = null;

        bool HasLogo => Logo is not null;

        protected void InvokeSaveEvent(object p) => OnSaveSuccessful?.Invoke(this, p);


        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        async Task Save() => await SaveAgency();


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

    public partial class Form_Add_Agency_ViewModel(IDBContextFactory contextFactory, IViewModelFactory viewModelFactory, INotificationHandler notificationHandler)
                       : Form_BaseAgency_ViewModel(contextFactory, viewModelFactory, notificationHandler) {
        public override FormSaveType SaveType => FormSaveType.Register;
        protected override bool ResetAgency() {
            if (base.ResetAgency()) {

                FirstName = LastName = MiddleName = ExtensionName = ContactDetails = Address = string.Empty;
                Logo = null;
                Contacts.Clear();
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

            if (MessageBox.Show("Are you sure you want to add this record?", "Review details before saving", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

            using (var context = contextFactory.CreateDbContext()) {

                var agencyToAdd = new Agency() {
                    Name = new Name() {
                        First = FirstName.TrimmedAndNullWhenEmpty()!,
                        Middle = MiddleName.TrimmedAndNullWhenEmpty(),
                        Last = LastName.TrimmedAndNullWhenEmpty(),
                        Extension = ExtensionName.TrimmedAndNullWhenEmpty()
                    },
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

                var result = context.Agencies.Add(agencyToAdd);
                await context.SaveChangesAsync();

                InvokeSaveEvent(result.Entity);
                NotificationHandler.RaiseNotification("New Agency Added!", "Agency Added: " + agencyToAdd.Name.ToString());
            }

            await base.SaveAgency();
        }
    }
    public partial class Form_Edit_Agency_ViewModel(IDBContextFactory contextFactory, IViewModelFactory viewModelFactory, INotificationHandler notificationHandler) : Form_BaseAgency_ViewModel(contextFactory, viewModelFactory, notificationHandler) {
        private Agency agency = null!;

        public Agency AgencyToEdit {
            get { return agency; }
            set {
                agency = value;
                SetDetails(agency);
            }
        }

        public override FormSaveType SaveType => FormSaveType.Edit;

        private void SetDetails(Agency agency) {
            FirstName = agency.Name.First;
            MiddleName = agency.Name.Middle.TrimmedAndNullWhenEmpty();
            LastName = agency.Name.Last.TrimmedAndNullWhenEmpty();
            ExtensionName = agency.Name.Extension.TrimmedAndNullWhenEmpty();

            Address = agency.Address.TrimmedAndNullWhenEmpty();
            Logo = agency.Logo?.ToImageSource();

            if (Contacts.Any()) Contacts.Clear();
            foreach (var c in agency.ContactDetails.Select(CreateContactItem).ToList())
                Contacts.Add(c);
        }

        Contact_Item_ViewModel CreateContactItem(ContactDetail contact) {

            var item = ViewModelFactory.Get<Contact_Item_ViewModel>();

            item.Id = contact.Id;
            item.ContactType = contact.Type;
            item.Value = contact.Value;
            item.IsDefault = contact.IsDefault;

            return item;
        }

        protected override bool ResetAgency() {
            if (base.ResetAgency()) {
                SetDetails(agency);
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

            if (MessageBox.Show("Are you sure you want to save these changes to the record?", "Review details before saving", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

            try {
                using var context = contextFactory.CreateDbContext();

                var agencyToEdit = await context.Agencies
                    .Include(a => a.ContactDetails)
                    .FirstOrDefaultAsync(a => a.Id == agency.Id);

                if (agencyToEdit is null) return;

                agencyToEdit.Name = new Name() {
                    First = FirstName.TrimmedAndNullWhenEmpty()!,
                    Middle = MiddleName.TrimmedAndNullWhenEmpty(),
                    Last = LastName.TrimmedAndNullWhenEmpty(),
                    Extension = ExtensionName.TrimmedAndNullWhenEmpty()
                };

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
                NotificationHandler.RaiseNotification("Changes on Agency Saved!", "Agency Editted: " + agencyToEdit.Name.ToString());

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            await base.SaveAgency();
        }
    }
}
