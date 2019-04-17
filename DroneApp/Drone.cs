using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using DroneLib;

namespace DroneApp
{
    class Drone
    {
        // TODO: Set the correct value here.
        const string AWS_ACCOUNT_ID = "???";

        public void DroneLand()
        {
            using (DroneApi api = new DroneApi())
            {
                api.Init();
                api.Land();
            }
        }

        public void DroneTakeOffAndLand()
        {
            Console.WriteLine("Look ma, I am flying a drone!");
            using (DroneApi api = new DroneApi())
            {
                api.Init();
                api.TakeOff();

                Thread.Sleep(5000);

                api.Land();
            }
        }

        public async Task TestDroneCommandFromSqsAsync(string queueName)
        {
            var config = new AmazonSQSConfig()
            {
                RegionEndpoint = RegionEndpoint.USWest2
            };

            string queueUrl = $"https://sqs.us-west-2.amazonaws.com/{AWS_ACCOUNT_ID}/" + queueName;
            int waitTimeMilliseconds = 20 * 1000;  // long polling to reduce lag time

            SqsQueue q = new SqsQueue(config);

            Console.WriteLine("---Look ma, I am getting SQS message!");
            Console.WriteLine("QueueName: {0}", queueName);
            Console.WriteLine("QueueUrl: {0}", queueUrl);
            Console.WriteLine("waitTimeMilliseconds: {0}", waitTimeMilliseconds);

            using (DroneApi droneApi = new DroneApi())
            {
                while (true)
                {
                    droneApi.Init();
                    Debug("Getting Drone Commands...");
                    var message = await q.GetMessage(queueUrl, queueName, waitTimeMilliseconds, 1);
                    string droneCommand = GetThenDeleteMessage(message);
                    HandleDroneCommand(droneApi, droneCommand);

                    Console.WriteLine();
                }
            }
        }

        private void HandleDroneCommand(DroneApi droneApi, string droneCommand)
        {
            switch (droneCommand)
            {
                case "takeoff":
                    droneApi.TakeOff();
                    break;
                case "land":
                    droneApi.Land();
                    break;
                default:
                    Debug("Unknown command: " + droneCommand);
                    break;
            }
        }

        public string GetThenDeleteMessage(Message message)
        {
            if (message != null)
            {
                Debug("command: " + message.MessageId + " " + message.Body);
                // todo: delete message from queue

                return message.Body;
            }

            return "";
        }

        public void Debug(string s)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now, s);
        }
    }
}