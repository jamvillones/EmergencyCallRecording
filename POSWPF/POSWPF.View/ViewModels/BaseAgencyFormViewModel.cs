using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.Domain.Data;
using ECR.Domain.Models;
using ECR.View.ViewModels;
using ECR.WPF.Utilities;
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
        private ImageSource? logo = null;

        protected IDBContextFactory contextFactory { get; init; }

        protected void InvokeSaveEvent(object p) {
            OnSaveSuccessful?.Invoke(this, p);
        }

        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        async Task Save() {
            await SaveAgency();
        }

        [RelayCommand]
        void RemoveLogo() {
            Logo = null;
        }

        protected abstract Task SaveAgency();

        [RelayCommand]
        void Reset() {

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

    public partial class AddAgencyFormViewModel : BaseAgencyFormViewModel {
        public AddAgencyFormViewModel(IDBContextFactory contextFactory) : base(contextFactory) {
        }

        private Agency agency = null!;

        public Agency Agency {
            get { return agency; }
            set {
                agency = value;
                Name = agency.Name;
                ContactDetails = agency.ContactInfo;
                Address = agency.Address;
                Logo = agency.Logo?.ToImageSource();
            }
        }

        protected override async Task SaveAgency() {
            ValidateAllProperties();
            if (HasErrors) {
                MessageBox.Show("Fill out required fields to continue", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = contextFactory.CreateDbContext()) {

                var agency = new Agency() {
                    Name = Name,
                    ContactInfo = ContactDetails,
                    Address = Address,
                    Logo = Logo.ToByteArray()
                };

                var result = context.Agency.Add(agency);

                await context.SaveChangesAsync();

                InvokeSaveEvent(result.Entity);
            }

            Close();
        }
    }
    public partial class EditAgencyFormViewModel : BaseAgencyFormViewModel {
        public EditAgencyFormViewModel(IDBContextFactory contextFactory) : base(contextFactory) {
        }

        protected override async Task SaveAgency() {

        }
    }
}
