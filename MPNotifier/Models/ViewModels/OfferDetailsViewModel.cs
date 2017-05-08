using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPNotifier.Models.ViewModels {
    public class OfferDetailsViewModel : INotifyPropertyChanged {
        private Guid id;

        public Guid Id {
            get => this.id;
            set {
                this.id = value;
                this.OnPropertyChanged();
            }
        }

        public string Company { get; set; }

        public string Title { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}