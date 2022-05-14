using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace TestService
{
    public sealed class Great : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private AppServiceConnection _connection;
        private string[] Names = new string[] { "Alice", "Bob" };
        private int[] Ages=new int[] { 21, 22 };

        

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += TaskInstance_Canceled;

            var detail = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            _connection = detail.AppServiceConnection;
            _connection.RequestReceived += _connection_RequestReceived;

        }

        private async void _connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var msgDeferral=args.GetDeferral();

            ValueSet msg = args.Request.Message;
            ValueSet returnData = new ValueSet();

            string command = msg["Command"] as string;
            int? GreatIndex = msg["ID"] as int?;

            if (GreatIndex.HasValue && GreatIndex.Value>=0 &&GreatIndex.Value<Names.GetLength(0))
            {
                switch (command)
                {
                    case "Name":
                        returnData.Add("Result", Names[GreatIndex.Value]);
                        returnData.Add("Status","ok");
                        break;

                    case "Age":
                        returnData.Add("Result", Ages[GreatIndex.Value]);
                        returnData.Add("Status", "ok");
                        break;

                    default:
                        returnData.Add("Status", "fail");
                        break;
                }
            }
            else
            {
                returnData.Add("fail", "Index out of range");
            }
            try
            {
                await args.Request.SendResponseAsync(returnData);
            }
            catch (Exception e)
            {

            }
            finally
            {
                msgDeferral.Complete();
            }
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (_deferral!=null)
            {
                _deferral.Complete();
            }
        }
    }
}