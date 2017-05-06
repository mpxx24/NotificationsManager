using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPNotifier.Models.ViewModels {
    public class ApplicationResultsViewModel : INotifyPropertyChanged {
        private ObservableCollection<JobOfferViewModel> offers;

        public int NumberOfNewOffers => this.offers.Count;

        public ObservableCollection<JobOfferViewModel> Offers {
            get => this.offers;
            set {
                this.offers = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}