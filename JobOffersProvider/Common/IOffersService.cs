using System.Collections.Generic;
using System.Threading.Tasks;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Common {
    public interface IOffersService {
        Task<IEnumerable<JobModel>> GetOffers();
    }
}