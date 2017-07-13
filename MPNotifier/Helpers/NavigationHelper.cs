using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MPNotifier.Helpers {
    public class NavigationHelper {
        private static Frame frame;

        public static Frame Frame {
            get => frame ?? (frame = Window.Current.Content as Frame);
            set => frame = value;
        }

        public static void Navigate(Type type, object parameters = null) {
            Frame.Navigate(type, parameters);
        }

        public static void Navigate(object parameters = null) {
            Frame.Navigate(typeof(MainPage), parameters);
        }
    }
}