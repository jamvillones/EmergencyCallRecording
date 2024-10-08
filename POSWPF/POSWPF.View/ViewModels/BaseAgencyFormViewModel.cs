using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.ViewModels;
using ECR.WPF.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECR.WPF.ViewModels {
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
                    ContactInfo = ContactDetails.TrimmedAndNullWhenEmpty(),
                    Address = Address.TrimmedAndNullWhenEmpty(),
                    Logo = Logo.ToByteArray()
                };

                var result = context.Agency.Add(agency);
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
                ContactDetails = agency.ContactInfo.TrimmedAndNullWhenEmpty();
                Address = agency.Address.TrimmedAndNullWhenEmpty();
                Logo = agency.Logo?.ToImageSource();
            }
        }

        protected override bool ResetAgency() {
            if (base.ResetAgency()) {
                Name = agency.Name;
                ContactDetails = agency.ContactInfo;
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
                var agencyToEdit = await context.Agency.FirstOrDefaultAsync(a => a.Id == agency.Id);

                if (agencyToEdit is null) return;

                agencyToEdit.Name = Name.TrimmedAndNullWhenEmpty()!;
                agencyToEdit.ContactInfo = ContactDetails.TrimmedAndNullWhenEmpty();
                agencyToEdit.Address = Address.TrimmedAndNullWhenEmpty();
                agencyToEdit.Logo = Logo.ToByteArray();

                await context.SaveChangesAsync();

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButton.OK, MessageBoxImage.Error);

            }

            await base.SaveAgency();
        }
    }
}
