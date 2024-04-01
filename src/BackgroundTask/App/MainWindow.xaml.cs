using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RegisterTask();
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            MyButton.Content = "Clicked";
        }
        static void  RegisterTask()
        {
            foreach ( var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name.Equals("ToastBGTask"))
                {
                    task.Value.Unregister(true);
                    System.Diagnostics.Debug.WriteLine("find registered task and canceled");
                    break;
                }
            }


            var builder = new BackgroundTaskBuilder
            {
                Name = "ToastBGTask",
                TaskEntryPoint = "BGTask.ToastBGTask"
            };
            builder.SetTrigger(new TimeTrigger(15, false));

            _ = builder.Register();

        }
    }
}
