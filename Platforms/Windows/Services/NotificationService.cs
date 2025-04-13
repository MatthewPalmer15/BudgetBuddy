using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BudgetBuddy.Platforms.Windows.Services
{
    public class WindowsNotificationService : INotificationService
    {

        public void SendNotification(string title, string content)
        {
            string toastXmlString = $@"
            <toast>
                <visual>
                    <binding template='ToastGeneric'>
                        <text>{title}</text>
                        <text>{content}</text>
                    </binding>
                </visual>
            </toast>";

            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(toastXmlString);

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier("BudgetBuddy").Show(toast);
        }
    }
}
