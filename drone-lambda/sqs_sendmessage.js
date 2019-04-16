// Load the AWS SDK for Node.js
var AWS = require('aws-sdk');
// Set the region 
AWS.config.update({region: 'us-east-1', accessKeyId: 'accessKeyId', secretAccessKey: 'secretAccessKey'});

// Create an SQS service object
var sqs = new AWS.SQS({apiVersion: '2012-11-05'});

var params = {
 DelaySeconds: 10,
//  MessageAttributes: {
//   "Title": {
//     DataType: "String",
//     StringValue: "The Whistler"
//    },
//   "Author": {
//     DataType: "String",
//     StringValue: "John Grisham"
//    },
//   "WeeksOn": {
//     DataType: "Number",
//     StringValue: "6"
//    }
//  },
 MessageBody: "Start drone",
 QueueUrl: "https://sqs.us-east-1.amazonaws.com/{account#}}/myqueue"
};

sqs.sendMessage(params, function(err, data) {
  console.log("sendMessage");

  if (err) {
    console.log("Error", err);
  } else {
    console.log("Success", data.MessageId);
  }
});