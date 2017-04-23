using System.Collections.Generic;
using System.Threading.Tasks;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Common {
    public interface IJobWebsiteTask {
        Task<IEnumerable<JobModel>> GetJobOffers();
    }
}