using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using DroneLib;

namespace DroneApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDroneCommandFromSqsAsync("DroneCommandQueue4.fifo");
            Console.Read();
            DroneLand();

            //DroneTakeOffAndLand();
        }

        static void DroneLand()
        {
            using (DroneApi api = new DroneApi())
            {
                api.Init();
                api.Land();
            }
        }

        static void DroneTakeOffAndLand()
        {
            Console.WriteLine("Look ma, I am flying a drone!");
            using (DroneApi api = new DroneApi())
            {
                api.Init();
                api.TakeOff();
                api.Land();
            }
        }

        static async Task TestDroneCommandFromSqsAsync(string queueName)
        {
            var config = new AmazonSQSConfig()
            {
                RegionEndpoint = RegionEndpoint.USWest2
            };

            string queueUrl = "https://sqs.us-west-2.amazonaws.com/{account#}/" + queueName;
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
                    Debug("Getting Drone Commands ... ");
                    var message = await q.GetMessage(queueUrl, queueName, waitTimeMilliseconds, 1);
                    string droneCommand = GetThenDeleteMessage(message);
                    HandleDroneCommand(droneApi, droneCommand);

                    Console.WriteLine();
                }
            }
        }

        private static void HandleDroneCommand(DroneApi droneApi, string droneCommand)
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

        static string GetThenDeleteMessage(Message message)
        {
            if (message != null)
            {
                Debug("command: " + message.MessageId + " " + message.Body);
                // todo: delete message from queue

                return message.Body;
            }

            return "";
        }

        static void Debug(string s)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now, s);
        }
    }
}