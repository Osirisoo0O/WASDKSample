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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.ApplicationModel.AppService;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppServiceConsumer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppServiceConnection _connection;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
            if (_connection == null) 
            {
                _connection = new AppServiceConnection();

                _connection.AppServiceName = "com.greatfive.great";
                _connection.PackageFamilyName = "3bd3f21f-c50b-49b6-aecc-59ff846ae0d2_kqnxptwqxttew";

                var status=await _connection.OpenAsync();
                if (status!=AppServiceConnectionStatus.Success)
                {
                    myTextBlock.Text = "Fail to Connect";
                    _connection = null;
                    return;
                }
            }

            int idx = int.Parse(myTextBox.Text);
            var msg = new ValueSet();
            msg.Add("Command", "Name");
            msg.Add("ID", idx);
            var response = await _connection.SendMessageAsync(msg);
            string result = "";
            if (response.Status==AppServiceResponseStatus.Success)
            {
                if (response.Message["Status"] as string =="ok")
                {
                    result = response.Message["Result"] as string;
                }
            }
            msg.Clear();
            msg.Add("Command", "Age");
            msg.Add("ID", idx);
            response = await _connection.SendMessageAsync(msg);

            if (response.Status==AppServiceResponseStatus.Success)
            {
                if (response.Message["Status"] as string =="ok")
                {
                    result += ":Age = " + response.Message["Result"] as string;
                }

            }

            myTextBox.Text = result;
        }
    }
}
