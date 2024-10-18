using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ECR.WPF.ViewModels {

    public class Notification_Item_ViewModel : ObservableObject {
        public Notification_Item_ViewModel() {
            _ = StartCountdown();
        }

        async Task StartCountdown() {
            await Task.Delay(TimeSpan.FromSeconds(3));

            OnExpire!(this);
        }
        public string Title { get; set; } = "Copied To Clipboard"!;
        public string Details { get; set; } = "Lorem ipsum odor amet, consectetuer adipiscing elit. Suscipit nibh enim hac mus dui. Sodales ultrices odio ligula tristique vulputate porta augue per mattis. Nostra quisque platea rutrum parturient dapibus augue. Quis elementum erat magna quisque cras arcu quis felis. Tristique eget tortor phasellus tristique praesent per."!;

        public event Action<Notification_Item_ViewModel>? OnExpire;
    }
}