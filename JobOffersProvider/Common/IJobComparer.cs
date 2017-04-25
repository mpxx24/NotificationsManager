using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Common {
    public interface IJobComparer {
        bool Compare(JobModel x, JobModel y);
    }
}