using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BGTask
{
    /// <summary>
    /// Toast任务
    /// </summary>
    public sealed class ToastBGTask : IBackgroundTask
    {
        /// <summary>
        /// 程序入口点
        /// </summary>
        /// <param name="taskInstance"></param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine($"后台任务：{taskInstance.Task.Name}启动中……");

            SendToast();

        }

        private static void SendToast()
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            XmlNodeList textElements = toastXml.GetElementsByTagName("text");
            textElements[0].AppendChild(toastXml.CreateTextNode("A toast example"));
            textElements[1].AppendChild(toastXml.CreateTextNode("You've changed timezones!"));
            ToastNotification notification = new(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }
    }
}