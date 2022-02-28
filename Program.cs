using System;
using System.Threading.Tasks;

namespace _30DaysOfMelissa
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Happy Birthday Mom!");
            var rm = new ReadEmail();
            var se = await StoreEmail.Init();
            var tm = new TextManager();
            if(Environment.GetEnvironmentVariable("3DM-GetMail", EnvironmentVariableTarget.User).ToLower() == "true")
            {
                var messages = rm.GetMessages();
                foreach (var msg in messages)
                {
                    await se.AddEmail(msg);
                    rm.ReadMessage(msg);
                }
            }
            if(Environment.GetEnvironmentVariable("3DM-SendSMS", EnvironmentVariableTarget.User) == "true")
            {
                var dbMessages = await se.GetUnsentMail(1);
                dbMessages.ForEach(async msg =>
                {
                    Console.WriteLine(msg.Message);
                    tm.SendText(msg.Message, msg.From);
                    await se.MarkMsgSent(msg);
                });
            }
        }
    }
}
