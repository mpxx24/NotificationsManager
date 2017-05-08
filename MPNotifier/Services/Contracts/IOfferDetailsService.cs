using System;
using MPNotifier.Models.ViewModels;

namespace MPNotifier.Services.Contracts {
    internal interface IOfferDetailsService {
        OfferDetailsViewModel GetOfferDetails(Guid offerId);
    }
}