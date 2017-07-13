using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MPNotifier.Helpers.Converter {
    public class BoolToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            var shouldBeVisible = (bool) parameter;

            return shouldBeVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            var isVisible = (Visibility) parameter;

            return isVisible == Visibility.Visible;
        }
    }
}