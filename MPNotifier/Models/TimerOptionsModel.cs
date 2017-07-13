namespace MPNotifier.Models {
    public class TimerOptionsModel {
        private int minutes { get; }

        public TimerOptionsModel(int minutes) {
            this.minutes = minutes;
        }

        public string DisplayValue {
            get {
                var hours = this.minutes / 60;
                var mins = this.minutes % 60;

                return hours >= 1 ? $"{hours}h {mins}min" : $"{mins}min";
            }
        }
    }
}