#Drone Lambda
Update with your own accesskey and secretkey, and queue url

AWS.config.update({ region: 'us-west-2', accessKeyId: 'accessKeyId', secretAccessKey: 'secretAccessKey' });
const QUEUE_URL = "https://sqs.us-west-2.amazonaws.com/{account#}/mydrone.fifo"