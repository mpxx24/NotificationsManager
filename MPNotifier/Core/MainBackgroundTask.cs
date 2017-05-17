using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace MPNotifier.Core {
    public sealed class MainBackgroundTask : IBackgroundTask {
        private readonly INotificationsLoader notificationsLoader;

        private BackgroundTaskDeferral deferral;

        public static string TaskName => "GetOffers";

        public MainBackgroundTask(INotificationsLoader notificationsLoader) {
            this.notificationsLoader = notificationsLoader;
        }

        public void Run(IBackgroundTaskInstance taskInstance) {
            this.deferral = taskInstance.GetDeferral();

            //get data async
            this.notificationsLoader.ShowToastNotification(5);

            this.deferral.Complete();
        }

        public static BackgroundTaskRegistration RegisterBackgroundTask(string taskEntryPoint, string name, IBackgroundTrigger trigger, IBackgroundCondition condition) {
            if (TaskRequiresBackgroundAccess(name)) {
                var requestTask = BackgroundExecutionManager.RequestAccessAsync();
            }

            var builder = new BackgroundTaskBuilder();

            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null) {
                builder.AddCondition(condition);
                builder.CancelOnConditionLoss = true;
            }

            var task = builder.Register();

            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(name);

            return task;
        }

        public static bool TaskRequiresBackgroundAccess(string name) {
            return name == "MainBackgroundTask";
        }
    }
}