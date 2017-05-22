using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MPNotifier.Helpers {
    public class NavigationHelper {
        private static Frame frame;

        public static Frame Frame {
            get {
                if (frame == null) {
                    frame = Window.Current.Content as Frame;
                }

                return frame;
            }
            set => frame = value;
        }

        public static void Navigate(Type type, object parameters = null) {
            Frame.Navigate(type, parameters);
        }
    }
}