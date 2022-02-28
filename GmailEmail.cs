using System;
using System.Collections.Generic;
using System.Text;

namespace _30DaysOfMelissa
{
    public class GmailEmail
    {
        public int EmailID { get; set; }
        public string EmailUid { get; set; }
        public bool IsSentSMS { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
    }
}
