using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MPNotifier {
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }
        private void Button_OnClick(object sender, RoutedEventArgs e) {
            var not = new NotificationsLoader();
            not.ShowToastNotification();
        }
    }
}