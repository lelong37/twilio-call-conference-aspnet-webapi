#region

using System.Net.Http;
using System.Text;
using System.Web.Http;
using Twilio.Mvc;
using Twilio.TwiML;

#endregion

namespace Twilio3.ApiControllers
{
    public class ConferenceController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Call(TwilioRequest voiceRequest)
        {
            var twilioResponse = new TwilioResponse();

            const string actionUrl = "http://longle.ngrok.com/api/Conference/User";

            twilioResponse.BeginGather(new {action = actionUrl, finishOnKey = "#", method = "POST"});

            twilioResponse.Say(@"
                Press 1 to join as listener, 
                Press 2 to join as speaker, 
                Press 3 to join as moderator",
                new {voice = "woman", numOfDigits = 1});

            twilioResponse.EndGather();

            return CreateResponseMessage(twilioResponse);
        }

        [HttpPost]
        public HttpResponseMessage User(VoiceRequest voiceRequest)
        {
            var twilioResponse = new TwilioResponse();

            switch (voiceRequest.Digits)
            {
                case "1":
                    twilioResponse.DialConference("myRoom", new {muted = true, startConferenceOnEnter = false});
                    break;
                case "2":
                    twilioResponse.DialConference("myRoom", new {startConferenceOnEnter = false});
                    break;
                case "3":
                    twilioResponse.DialConference("myRoom", new { startConferenceOnEnter = true, endConferenceOnExit = true, muted = false });
                    break;
            }

            return CreateResponseMessage(twilioResponse);
        }

        private HttpResponseMessage CreateResponseMessage(TwilioResponse twilioResponse)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(twilioResponse.ToString(), Encoding.UTF8, "text/xml")
            };
        }
    }
}