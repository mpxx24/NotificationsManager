using System;
using Windows.Data.Xml.Dom;
using Microsoft.Toolkit.Uwp.Notifications;

namespace MPNotifier {
    public static class Notifications {
        public static XmlDocument GetNotificationContent(string title, string text, string companyLogoLink, string offerAddress) {
            var toastContent = getNotificationContent(title, text, companyLogoLink, offerAddress);
            return toastContent.GetXml();
        }

        private static ToastContent getNotificationContent(string title, string text, string companyLogoLink, string offerAddress) {
            var content = new ToastContent {
                Launch = offerAddress,
                ActivationType = ToastActivationType.Foreground,

                Visual = new ToastVisual {
                    BindingGeneric = new ToastBindingGeneric {
                        Children = {
                            new AdaptiveText {
                                Text = title
                            },

                            new AdaptiveText {
                                Text = text
                            }
                        },

                        AppLogoOverride = new ToastGenericAppLogo {
                            Source = companyLogoLink
                        }
                    }
                },

                Actions = new ToastActionsCustom {
                    Buttons = {
                        new ToastButton("Go to offer", offerAddress) {
                            ActivationType = ToastActivationType.Foreground
                        }
                    }
                },

                Audio = new ToastAudio {
                    Src = new Uri("ms-winsoundevent:Notification.Reminder")
                }
            };

            return content;
        }
    }
}