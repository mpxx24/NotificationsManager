using System;

namespace MPNotifier.Models.ViewModels {
    public class OfferDetailsViewModel {
        public Guid Id { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }
    }
}