using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace _30DaysOfMelissa
{
    public class TextManager
    {
        private static readonly string _accountSID = Environment.GetEnvironmentVariable("TwilioAccountSID", EnvironmentVariableTarget.User);
        private static readonly string _authToken = Environment.GetEnvironmentVariable("TwilioAuthToken", EnvironmentVariableTarget.User);
        private static readonly string _fromPhoneNumber = Environment.GetEnvironmentVariable("TwilioPhoneNumber", EnvironmentVariableTarget.User);
        private static readonly string _toPhoneNumber = Environment.GetEnvironmentVariable("3DM-PhoneNumber", EnvironmentVariableTarget.User);

        public TextManager()
        {
            TwilioClient.Init(_accountSID, _authToken);
        }

        public void SendText(string msg, string from)
        {
            var toNumber = new PhoneNumber(_toPhoneNumber);
            var fromNumber = new PhoneNumber(_fromPhoneNumber);

            var textMessage = MessageResource.Create(to: toNumber, from: fromNumber,
                body: $"Message From: {from} - {msg}");
        }
    }
}
