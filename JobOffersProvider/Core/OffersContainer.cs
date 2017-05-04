using System.Linq;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Core {
    public class OffersContainer {
        public static IQueryable<JobModel> JobOfferModels { get; set; }
    }
}