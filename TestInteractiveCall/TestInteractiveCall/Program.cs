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


//setup the API key.
DilloVoiceClient.Init(CUSTOMER_CODE, API_KEY);

// open the message that has to be sent
string MessagePayload= File.ReadAllText("..\\..\\..\\Request_issue_call.json", Encoding.UTF8);

// create the request
OutboundVoiceRequest outboundVoiceRequest = new OutboundVoiceRequest
{
    SendActionType = "Voice",
    MessagePayload = MessagePayload
};

// send the request
OutboundVoicePostResponse postResponse = await outboundVoiceRequest.SendAsync();

// wait unitill the call has finishd
OutboundVoiceGetResponse getResponse = await postResponse.GetAsync();
while (getResponse.Result.Status.PrimaryStatus.Code != 200){
    getResponse = await postResponse.GetAsync();
    Thread.Sleep(1000);
}

foreach (CollectedDigits digit in getResponse.Result.CollectedDigits)
{
    if (digit.IsSuccess == true && digit.Node == 4)
    {
        Console.WriteLine(digit.Digits);
    }
}

