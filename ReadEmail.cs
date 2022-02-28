using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AE.Net.Mail;

namespace _30DaysOfMelissa
{
    public class ReadEmail
    {
        private ImapClient _ic;
        private static readonly string _clientURL = "imap.gmail.com";
        private static readonly string _mailbox = "TestMeJacob";
        private static int _characterLimit = 200;
        public ReadEmail()
        {
            string email = Environment.GetEnvironmentVariable("3DM-Email", EnvironmentVariableTarget.User);
            string psw = Environment.GetEnvironmentVariable("3DM-Password", EnvironmentVariableTarget.User);

            //Needed because the library being used has an encoding that .NET Core does not have out of the box
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _ic = new ImapClient(_clientURL, email, psw, AuthMethods.Login, 993, true);
        }

        public List<GmailEmail> GetMessages()
        {
            var msgList = new List<GmailEmail>() { };
            _ic.SelectMailbox(_mailbox);
            var test = _ic.SearchMessages(SearchCondition.Unseen());
            foreach (var msg in test)
            {
                var msgValue = msg.Value;
                msgList.Add(new GmailEmail()
                {
                    EmailUid = msgValue.Uid,
                    From = msgValue.From.Address,
                    Message = msgValue.Body.Substring(0, msgValue.Body.Length > _characterLimit ? _characterLimit : msgValue.Body.Length)
                });

            }

            return msgList;
        }

        public void ReadMessage(GmailEmail msg)
        {
            var gmailMsg = _ic.GetMessage(msg.EmailUid);
            _ic.SetFlags(Flags.Seen, gmailMsg);
        }
    }
}
