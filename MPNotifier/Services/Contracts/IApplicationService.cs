using MPNotifier.Models;

namespace MPNotifier.Services.Contracts {
    public interface IApplicationService {
        void PrepareApplicationData(ApplicationSettingsModel settings);
    }
}