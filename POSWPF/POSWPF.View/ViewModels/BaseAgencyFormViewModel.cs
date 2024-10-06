using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.View.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECR.WPF.ViewModels
{
    public abstract partial class BaseAgencyFormViewModel : ObservableValidator, ICloseableObject
    {
        public event EventHandler? OnClose;

        const string REQUIRED_FIELD_STRING = "*Requred";
        [RelayCommand]
        public void Close()
        {
            this.OnClose?.Invoke(this, EventArgs.Empty);
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = REQUIRED_FIELD_STRING)]
        private string name = null!;

        [ObservableProperty]
        private string contactDetails = null!;

        [ObservableProperty]
        private string address = null!;


        [ObservableProperty]
        private ImageSource? logo = null;

        protected void InvokeSaveEvent(object p)
        {
            OnSaveSuccessful?.Invoke(this, p);
        }

        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        async Task Save()
        {
            await SaveAgency();
        }

        protected abstract Task SaveAgency();

        [RelayCommand]
        void Reset()
        {

        }

        [RelayCommand]
        void PickImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new()
            {
                Filter = "Image Files | *.jpg;*.jpeg;*.png;"
            };

            bool? result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                //extension = Path.GetExtension(dlg.FileName);
                Logo = new BitmapImage(new Uri(filename));
            }
        }
    }

    public partial class AddAgencyFormViewModel : BaseAgencyFormViewModel
    {
        protected override async Task SaveAgency()
        {

        }
    }
        public partial class EditAgencyFormViewModel : BaseAgencyFormViewModel
        {
            protected override async Task SaveAgency()
            {

            }
        }
}
