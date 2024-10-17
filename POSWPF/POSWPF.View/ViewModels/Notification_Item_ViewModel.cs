using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ECR.WPF.ViewModels {

    public class Notification_Item_ViewModel : ObservableObject {
        public Notification_Item_ViewModel() {
            _ = StartCountdown();
        }

        async Task StartCountdown() {
            await Task.Delay(2000);

            OnExpire!(this);
        }
        public string Title { get; set; } = "Copied To Clipboard"!;
        public string Details { get; set; } = "Lorem Ipsum Dolor Amet"!;

        public event Action<Notification_Item_ViewModel>? OnExpire;
    }
}