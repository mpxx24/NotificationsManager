namespace MPNotifier {
    public static class Notifications {
        public static string GetNotification(string title, string text, string companyLogoLink, string offferAddress) {
            return "<toast launch=\"app-defined-string\">" +
                   "<visual>" +
                   "<binding template=\"ToastGeneric\">" +
                   $"<text>{title}</text>" +
                   $"<text>{text}</text>" +
                   $"<image placement=\"AppLogoOverride\" src=\"{companyLogoLink}\" />" +
                   "</binding>" +
                   "</visual>" +
                   "</toast>";
        }
    }
}