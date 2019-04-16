# telloDrone
tello drone control integrated with AWS SQS FIFO queue

# github source control 
https://github.com/liquid-studios-seattle/alexaDrone

# SQS Fifo queues
https://console.aws.amazon.com/sqs/home?region=us-west-2

# how to add AWS references
Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution ... -> Browse -> Type in "awssdk"

.csproj file will be updated, for example: 
  <ItemGroup>
    <PackageReference Include="AWSSDK.Core" Version="3.3.100.5" />
    <PackageReference Include="AWSSDK.SQS" Version="3.3.100.5" />
  </ItemGroup>

# aws credential file
C:\Users\{username}\.aws\credentials

# Alexa console
https://developer.amazon.com/alexa

Create an account 

Alexa -> Alexa Skill Kit

# Alexsa Skill Definition
Schema definition for intreaction model

Skill ID: 
amzn1.ask.skill.bb4045e6-b3e8-4133-b650-72923c5980f1

lambda arn: 
arn:aws:lambda:us-east-1:{account#}:function:MyDroneLambda

how to update skill json: 
https://stackoverflow.com/questions/42559056/is-there-a-way-to-use-the-cli-to-configure-an-alexa-skill