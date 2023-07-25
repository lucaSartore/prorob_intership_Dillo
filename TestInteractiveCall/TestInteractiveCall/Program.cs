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

const string API_KEY = "B14604DA5FA87D1BD5ABBF193F68948746673C16356328F7C7928E05A9AF2D4A";
const string CUSTOMER_CODE = "783ec624-8136-4dc5-864a-0fbeb9b0a925";



/*
var options = new RestClientOptions("https://api.dillo.cloud")
{
    MaxTimeout = -1,
};

var client = new RestClient(options);

var request = new RestRequest("/api/sendaction", Method.Post);


request.AddHeader("ApiKey", API_KEY);
request.AddHeader("CustomerCode", CUSTOMER_CODE);

request.AddHeader("Content-Type", "application/json");

var body = @"{

" + "\n" +

@"    ""SendActionType"": ""Voice"",

" + "\n" +

@"    ""MessagePayload"": ""{\""ChannelBehavior\"":{\""ChannelBehaviorNodes\"":{\""0\"":{\""ActionType\"":\""Say\"",\""ChannelBehaviorAction\"":{\""Text\"":\""Testo Messaggio di alert. Il nodo 2 è di tipo play, l'ho inserito solo di esempio ma lo puoi saltare. Digita 1 per confermare l'intervento.\"",\""LanguageCode\"":\""it-IT\"",\""VoiceId\"":\""Bianca\""},\""NextNode\"":3},\""2\"":{\""ActionType\"":\""Play\"",\""ChannelBehaviorAction\"":{\""Url\"":\""https://ahdjkjjjjj.blob.core.windows.net/aaaaa-customeraudiofile/cpne_b6c11f57-aac3-49b5-9a09-d0a1c122f0e7.wav\""},\""NextNode\"":3},\""3\"":{\""ActionType\"":\""Menu\"",\""ChannelBehaviorAction\"":{\""Digits\"":{\""1\"":{\""NextNode\"":10}},\""Timeout\"":10,\""RecordMaxLength\"":5,\""PlayBeep\"":true,\""Recordings\"":[],\""StorageKey\"":\""ConfermaOrdine\"",\""VoicesExactMatch\"":false,\""OptionStatusCallbackUrl\"":\""https://realtimedigit.it\"",\""OptionStatusCallbackMethod\"":\""POST\"",\""RecordingStoragePeriodCode\"":\""3-M\"",\""Transcribe\"":false,\""LanguageCode\"":\""it-IT\""}},\""10\"":{\""ActionType\"":\""Say\"",\""ChannelBehaviorAction\"":{\""Text\"":\""Testo dopo la conferma\"",\""LanguageCode\"":\""it-IT\"",\""VoiceId\"":\""Bianca\""}}}},\""TelephoneNumber\"":\""+393703188051\"",\""StatusCallbackUrl\"":\""https://www.testcallback.it\"",\""Sender\"":\""+393391003110\""}""

" + "\n" +

@"}";
request.AddStringBody(body, DataFormat.Json);

RestResponse response = await client.ExecuteAsync(request);

Console.WriteLine("Api response: " + response.Content);


var responseContent_str = response.Content;*/

var responseContent_str = "{ \"id\":\"c59be024-b63f-42d9-a08d-ee404de12a52\"}";

ResponseContent? responseContent_row = JsonSerializer.Deserialize<ResponseContent>(responseContent_str);
if (responseContent_row == null)
{
    throw new ArgumentNullException(nameof(responseContent_row));
}
ResponseContent responseContent = responseContent_row;

Console.WriteLine("Api response: " + responseContent.id);


DilloVoiceClient.Init(CUSTOMER_CODE, API_KEY);

OutboundVoicePostResponse postResponse = new OutboundVoicePostResponse
{
    Id = new Guid("c59be024-b63f-42d9-a08d-ee404de12a52"),
    IsSuccess = true,
};

Console.WriteLine(postResponse.IsSuccess + " " + postResponse.Id.HasValue);

OutboundVoiceGetResponse getResponse = await postResponse.GetAsync();

Console.WriteLine("Call response: " + getResponse.Result.TelephoneNumber);

Console.WriteLine(JsonSerializer.Serialize<OutboundVoiceRequestResult>(getResponse.Result));

// struct to use for json desirialization
public class ResponseContent
{
    public string id { get; set; }
}