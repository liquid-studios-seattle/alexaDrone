using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace DroneLib
{
    public class SqsQueue
    {
        private readonly AWSCredentials _credentials;
        private readonly RegionEndpoint _region;
        private AmazonSQSConfig _config;

        public SqsQueue(AmazonSQSConfig config)
        {
            _config = config;
        }

        public SqsQueue(string accessKey, string secreteKey, RegionEndpoint region)
        {
            _credentials = new BasicAWSCredentials(accessKey, secreteKey);
            _region = region;
        }

        public async Task<Message> GetMessage(string queueUrl, string queueName, int waitTimeMilliseconds, int maxNumberOfMessages)
        {
            Message message = null;
         
            var request = new ReceiveMessageRequest(queueName)
            {
                QueueUrl = queueUrl, 
                MaxNumberOfMessages = maxNumberOfMessages,
                WaitTimeSeconds = (int)TimeSpan.FromMilliseconds(waitTimeMilliseconds).TotalSeconds
            };

            using (var client = CreateAmazonSQSClient())
            {
                //Debug("before ReceiveMessageAsync");

                var response = await client.ReceiveMessageAsync(request);
                //Debug("after ReceiveMessageAsync");

                if (response.HttpStatusCode != HttpStatusCode.OK) return message;

                if (response.ContentLength == 0) return message;

                if (!response.Messages.Any()) return message;

                message = response.Messages[0];
            }

            return message;
        }

        private AmazonSQSClient CreateAmazonSQSClient()
        {
            if (_config != null)
                return new AmazonSQSClient(_config);
            else
                return new AmazonSQSClient(_credentials, _region);
        }

        private void Debug(string s)
        {
            Console.WriteLine("{0}: {1}", DateTime.Now, s);
        }
    }
}