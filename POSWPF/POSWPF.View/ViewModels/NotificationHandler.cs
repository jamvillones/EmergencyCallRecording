using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR.WPF.ViewModels {
    public partial class NotificationHandler : ObservableObject {

        public void RaiseNotification(string title, string description) {
            var newNotif = new Notification_Item_ViewModel() { Title = title, Details = description };
            newNotif.OnExpire += CloseItem;
            Notifications.Add(newNotif);
        }

        public ObservableCollection<Notification_Item_ViewModel> Notifications { get; } = [];

        [RelayCommand]
        void CloseItem(Notification_Item_ViewModel item) {
            Notifications.Remove(item);
        }
    }
}
