using Dillo.Voice;
using System.Text;

namespace issues_collection
{

   
    public interface IIssueState
    {
        public static string ToString()
        {
            return "";
        }
    }

    public class IssueCollector<T> where T : IIssueState
    {
        // key and value for the dillo API
        private const string API_KEY = "B14604DA5FA87D1BD5ABBF193F68948746673C16356328F7C7928E05A9AF2D4A";
        private const string CUSTOMER_CODE = "783ec624-8136-4dc5-864a-0fbeb9b0a925";

        // the message that will be sent to the user
        private string? MessagePayload = null;
        private ISet<T> List_Of_States;
        // default constructor
        public IssueCollector(ISet<T> list_of_states)
        {
            // initialize the API
            DilloVoiceClient.Init(CUSTOMER_CODE, API_KEY);
            // initialize the message
            IssueCollector<T>.initialize_message_payload();
        }

        public int GetCode()
        {
            return 0;
        }

        private void initialize_message_payload()
        {
            if (MessagePayload == null)
            { 
                // load the template fule
                MessagePayload = File.ReadAllText("..\\..\\..\\Request_issue_call.json", Encoding.UTF8);
                
                // create the description
                foreach(int stae in T.GetAllStates())
                {

                }


            }
        }
    }

}