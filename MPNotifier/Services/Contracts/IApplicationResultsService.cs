using System;
using System.Collections.Generic;
using MPNotifier.Models.ViewModels;

namespace MPNotifier.Services.Contracts {
    public interface IApplicationResultsService {
        IEnumerable<JobOfferViewModel> GetAllJobOfferViewModels();

        OfferDetailsViewModel GetOfferDetails(Guid offerId);
    }
}