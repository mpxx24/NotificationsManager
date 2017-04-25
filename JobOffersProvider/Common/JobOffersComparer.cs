using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Common {
    public class JobOffersComparer : IJobComparer {
        public bool Compare(JobModel x, JobModel y) {
            return x.Company == y.Company && x.Title == y.Title;
        }
    }
}