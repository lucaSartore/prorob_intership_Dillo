// dotnet add package Dillo.Voice --version 1.9.0
using Dillo.Voice;
using Dillo.Voice.Dto;
using Dillo.Voice.Extensions;
using Dillo.Voice.Models.Outbound;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text;

namespace issues_collection
{

   
    public enum MyEnum
    {
        Pause = 3,
        Error = 2,
        Maintenance = 1
    }



    public class IssueCollector<T> where T : Enum
    {
        // key and value for the dillo API
        private const string API_KEY = "B14604DA5FA87D1BD5ABBF193F68948746673C16356328F7C7928E05A9AF2D4A";
        private const string CUSTOMER_CODE = "783ec624-8136-4dc5-864a-0fbeb9b0a925";

        private static bool _isClientInit = false;

        // the message that will be sent to the user
        private string _messagePayload;

        private Task<OutboundVoicePostResponse>? _postResponse = null;

        // default constructor
        public IssueCollector()
        {
            if (!_isClientInit){
                // initialize the API
                DilloVoiceClient.Init(CUSTOMER_CODE, API_KEY);
                _isClientInit = true;
            }


            // read the file with the basic template
            _messagePayload = File.ReadAllText("..\\..\\..\\Request_issue_call.json", Encoding.UTF8);

            // generate the new string of enum
            string message = "";

            foreach (T state in Enum.GetValues(typeof(T)))
            {
                message += " Il codice per " + state.ToString() + " è " + Convert.ToInt32(state) + ", ";
                //Console.WriteLine(state);
            }

            // insert the custom message inside the template
            _messagePayload = _messagePayload.Replace("Il codice per Pausa è 1. il codice per Manutenzione è 2. il codice per Lavaggio è 3", message);
        }

        public void SendRequest()
        {

            if (_postResponse != null){
                throw new Exception("you can't send a new request while an other one is on going");
            }

            OutboundVoiceRequest outboundVoiceRequest = new OutboundVoiceRequest
            {
                SendActionType = "Voice",
                MessagePayload = _messagePayload
            };
            _postResponse = outboundVoiceRequest.SendAsync();
        }

        public async Task<T> GetCode()
        {
            if (_postResponse == null){
                throw new Exception("before getting the input is necessay to call SendRequest");
            }

            // wait unitill the call has finishd
            OutboundVoiceGetResponse getResponse = await (await _postResponse).GetAsync();
            while (getResponse.Result.Status.PrimaryStatus.Code != 200)
            {
                getResponse = await (await _postResponse).GetAsync();
                Thread.Sleep(1000);
            }

            _postResponse = null;

            int status_code = 0;

            foreach (CollectedDigits digit in getResponse.Result.CollectedDigits)
            {
                // collect only the digits taken in node 4
                if (digit.IsSuccess == true && digit.Node == 4)
                {
                    // convert the string to integer
                    if (!Int32.TryParse(digit.Digits, out status_code))
                    {
                        throw new Exception("The number digits typed by the user is not a valid number");
                    }
                }
            }

            // try convert the number into the enum
            if (Enum.IsDefined(typeof(T), status_code))
            {
                return (T)(object)status_code;
            }
            else
            {
                throw new Exception("the code insered by the user is invalid");
            }
        }

    }

}