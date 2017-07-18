using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MPNotifier.Models;
using MPNotifier.Views;

namespace MPNotifier {
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
            this.CurrentFrame.Navigate(typeof(Settings));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }

        private void LeftSideMenu_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (this.Results.IsSelected) {
                //this.CurrentFrame.Navigate(typeof(ApplicationResults), new NavigationModel { Parameter = null });
            } else if (this.Settings.IsSelected) {
                this.CurrentFrame.Navigate(typeof(Settings));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var navigationModel = e.Parameter as NavigationModel;

            if (navigationModel != null) {
                this.CurrentFrame.Navigate(navigationModel.ViewType, navigationModel);
            }
        }
    }
}