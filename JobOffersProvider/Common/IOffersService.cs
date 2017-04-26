using System.Collections.Generic;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Common {
    public interface IOffersService {
        IEnumerable<JobModel> GetOffers();
    }
}