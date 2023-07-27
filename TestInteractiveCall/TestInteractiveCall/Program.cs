// libraries needed:
// dotnet add package Dillo.Voice
// dotnet add package RestSharp


using Dillo.Voice.Dto;
using Dillo.Voice;
using RestSharp;
using RestSharp.Authenticators;
using Dillo.Voice.Extensions;
using System.Text.Json;
using System.Runtime.InteropServices;
using Dillo.Voice.Models.Outbound;
using Dillo.Voice.Builders;
using Dillo.Voice.Constants.Say;
using System.Threading;
using Dillo.Voice.Constants.Transcribe;
using Dillo.Voice.Models;
using Dillo.Voice.Models.Actions;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using System.ComponentModel;
using System.IO;
using System.Text;
using Dillo.Voice.Constants.SendAction;

const string API_KEY = "B14604DA5FA87D1BD5ABBF193F68948746673C16356328F7C7928E05A9AF2D4A";
const string CUSTOMER_CODE = "783ec624-8136-4dc5-864a-0fbeb9b0a925";


// the voice that will be used for the call
SayVoice voice = new SayVoice
{
    LanguageCode = "it-IT",
    VoiceId = "IsabellaNeural"
};

//setup the API key.
DilloVoiceClient.Init(CUSTOMER_CODE, API_KEY);
/*
MenuBuilder menuBuilder = new MenuBuilder(timeout: 10)
        .SetIntro(text: "Hello, for Sales, press 1. For Technical Support, press 2. To speak with an operator, press 3.", StandardVoices.EnglishUSMaleMatthew)
        .AddDigit("1", ActionBuilder.Say(text: "You pressed 1 and chose Sales.", voice: StandardVoices.EnglishUSMaleMatthew))
        .AddDigit("2", ActionBuilder.Say(text: "You pressed 2 and chose Technical Support.", voice: StandardVoices.EnglishUSMaleMatthew))
        .RepeatOnNoDigit()
        .RepeatOnWrongDigit();

ChannelBehaviorBuilder channelBehaviorBuilder = new ChannelBehaviorBuilder()
  .Menu(menuBuilder)
  .Collect(finishOnKey: "#", timeout: 30, playBeep: true, numberOfDigits: 4);


// decide sender and reciver
OutboundVoiceRequestBuilder outboundRequestBuilder = new OutboundVoiceRequestBuilder(
    sender: "+393391003110",
    telephoneNumber: "+393703188051",
    anonymizeInHours: 1.5)
.SetChannelBehavior(channelBehaviorBuilder);

// send the message
OutboundVoiceRequest outboundVoiceRequest = outboundRequestBuilder.Build();*/

string MessagePayload= File.ReadAllText("..\\..\\..\\Request_issue_call.json", Encoding.UTF8);

//Console.WriteLine(MessagePayload);


/*
using (StreamWriter writer = new StreamWriter("C:\\Users\\luca.sartore\\Desktop\\SendAction.json"))
{
    writer.WriteLine(outboundVoiceRequest.SendActionType);
}
using (StreamWriter writer = new StreamWriter("C:\\Users\\luca.sartore\\Desktop\\MessagePayload.json"))
{
    writer.WriteLine(outboundVoiceRequest.MessagePayload);
}

throw new Exception();*/


OutboundVoicePostResponse postResponse = await outboundVoiceRequest.SendAsync();

Console.WriteLine(JsonSerializer.Serialize<OutboundVoicePostResponse>(postResponse));


// wait 45 seconds
Thread.Sleep(35*1000);

/*
OutboundVoicePostResponse postResponse = new OutboundVoicePostResponse
{
    Id = new Guid("788cbeb7-797b-4313-a073-e16424ef39d8"),
    IsSuccess = true,
};*/





// wait for the call to finish
OutboundVoiceGetResponse getResponse = await postResponse.GetAsync();
while (getResponse.Result.Status.PrimaryStatus.Code != 200){
    getResponse = await postResponse.GetAsync();
    Thread.Sleep(1000);
}

//Console.WriteLine("Call response: " + getResponse.Result.TelephoneNumber);

Console.WriteLine(JsonSerializer.Serialize<OutboundVoiceRequestResult>(getResponse.Result));


string result = JsonSerializer.Serialize<OutboundVoiceRequestResult>(getResponse.Result);

using (StreamWriter writer = new StreamWriter("C:\\Users\\luca.sartore\\Desktop\\file.json"))
{
    writer.WriteLine(result);
}