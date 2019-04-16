/* eslint-disable  func-names */
/* eslint quote-props: ["error", "consistent"]*/
/**
 * This sample demonstrates a simple skill built with the Amazon Alexa Skills
 * nodejs skill development kit.
 * This sample supports multiple lauguages. (en-US, en-GB, de-DE).
 * The Intent Schema, Custom Slots and Sample Utterances for this skill, as well
 * as testing instructions are located at https://github.com/alexa/skill-sample-nodejs-fact
 **/

'use strict';
const Alexa = require('alexa-sdk');
var AWS = require('aws-sdk');
// Set the region 
AWS.config.update({ region: 'us-west-2', accessKeyId: 'accessKeyId', secretAccessKey: 'secretAccessKey' });

// Create an SQS service object
var sqs = new AWS.SQS({apiVersion: '2012-11-05'});

const QUEUE_URL = "https://sqs.us-west-2.amazonaws.com/{account#}}/myqueue.fifo"

//=========================================================================================================================================
//TODO: The items below this comment need your attention.
//=========================================================================================================================================

//Replace with your app ID (OPTIONAL).  You can find this value at the top of your skill's page on http://developer.amazon.com.
//Make sure to enclose your value in quotes, like this: const APP_ID = 'amzn1.ask.skill.bb4045e6-b3e8-4133-b650-72923c5980f1';
//const APP_ID = undefined;
const APP_ID = 'amzn1.ask.skill.bb4045e6-b3e8-4133-b650-72923c5980f1';

const SKILL_NAME = 'drone skill';
const HELP_MESSAGE = 'You can say takeoff or land';
const HELP_REPROMPT = 'What can I help you with?';
const STOP_MESSAGE = 'Goodbye!';

//=========================================================================================================================================
//Editing anything below this line might break your skill.
//=========================================================================================================================================


const handlers = {
    'LaunchRequest': function () {
        const startingSession = 'say takeoff or land when ready';
        this.response.cardRenderer(SKILL_NAME, startingSession);
        this.response.speak(startingSession);
        this.response.shouldEndSession(false);
        this.emit(':responseReady');
    },
    'TakeoffIntent': function () {
		var speechOutput = "Drone takeoff";

        this.response.cardRenderer(SKILL_NAME, speechOutput);
        this.response.speak(speechOutput);
        this.response.shouldEndSession(false);

        var message = "takeoff " + new Date();
        var params = {
            MessageBody: message,
            MessageGroupId: "1",
            QueueUrl: QUEUE_URL,
        };

        sqs.sendMessage(params, function (err, data) {
            console.log("sendMessage");

            if (err) {
                console.log("Error", err);
            } else {
                console.log("Success", data.MessageId);
            }
        });
        this.emit(':responseReady');
    },
    'AMAZON.HelpIntent': function () {
        const speechOutput = HELP_MESSAGE;
        const reprompt = HELP_REPROMPT;

        this.response.speak(speechOutput).listen(reprompt);
        this.emit(':responseReady');
    },
    'AMAZON.CancelIntent': function () {
        this.response.speak(STOP_MESSAGE);
        this.emit(':responseReady');
    },
    'AMAZON.StopIntent': function () {
        this.response.speak(STOP_MESSAGE);
        this.emit(':responseReady');
    },
    'Unhandled': function () {
        this.emit(':ask', HELP_MESSAGE, HELP_MESSAGE);
    },
};

exports.handler = function (event, context, callback) {
    const alexa = Alexa.handler(event, context, callback);
    alexa.APP_ID = APP_ID;
    alexa.registerHandlers(handlers);
    alexa.execute();
};
