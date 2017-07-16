using System;
using JobOffersProvider.Common;
using MPNotifier.Helpers;

namespace MPNotifier.Models {
    public class TimerOptionsModel {
        public TimeIntervalType IntervalType { get; }

        public TimerOptionsModel(TimeIntervalType intervalType) {
            this.IntervalType = intervalType;
        }

        public string DisplayValue {
            get {
                var minutes = 0;
                switch (this.IntervalType) {
                    case TimeIntervalType.HalfAnHour:
                        minutes = 30;
                        break;
                    case TimeIntervalType.Hour:
                        minutes = 60;
                        break;
                    case TimeIntervalType.HourAndAHalf:
                        minutes = 90;
                        break;
                }

                var hours = minutes / 60;
                var mins = minutes % 60;

                return hours >= 1 
                    ? $"{hours}{StringLocalizationsHelper.HourAbbreviation} {mins}{StringLocalizationsHelper.MinutesAbbreviation}" 
                    : $"{mins}{StringLocalizationsHelper.MinutesAbbreviation}";
            }
        }
    }
}