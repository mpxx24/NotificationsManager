namespace MPNotifier.Models {
    public class TimerOptionsModel {
        public int Minutes { get; set; }

        public TimerOptionsModel(int minutes) {
            this.Minutes = minutes;
        }

        public string DisplayValue {
            get {
                var hours = this.Minutes / 60;
                var minutes = this.Minutes % 60;

                return hours >= 1 ? $"{hours}h {minutes}min" : $"{minutes}min";
            }
        }
    }
}