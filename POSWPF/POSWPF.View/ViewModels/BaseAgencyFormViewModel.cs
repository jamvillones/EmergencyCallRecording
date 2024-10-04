using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECR.View.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.ViewModels {
    public abstract partial class BaseAgencyFormViewModel : ObservableValidator, ICloseableObject {
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
        private string contactDetails = null!;

        [ObservableProperty]
        private string address = null!;


        [ObservableProperty]
        private byte[] logo = [];

        protected void InvokeSaveEvent(object p) {
            OnSaveSuccessful?.Invoke(this, p);
        }

        public event EventHandler<object>? OnSaveSuccessful;

        [RelayCommand]
        void Save() {

        }

        [RelayCommand]
        void Reset() {

        }
    }

    public partial class AddAgencyFormViewModel : BaseAgencyFormViewModel {

    }

    public partial class EditAgencyFormViewModel : BaseAgencyFormViewModel {

    }
}
