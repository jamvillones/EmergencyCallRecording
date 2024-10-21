using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.ViewModels {
    interface IModalViewer {

        ICloseableObject CurrentModal { get; }

        void SetModal(ICloseableObject closeableObject);
    }
    partial class ModalViewer : ObservableObject, IModalViewer {

        [ObservableProperty]
        ICloseableObject? currentModal = null;

        public void SetModal(ICloseableObject closeableObject) {
            CurrentModal = closeableObject;
            CurrentModal.OnClose += CurrentModal_OnClose;
        }

        private void CurrentModal_OnClose(object? sender, EventArgs e) {
            CurrentModal = null;
        }
    }
}
