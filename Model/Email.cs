using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleBuilder.Model
{
    public class Email
    {
        private Person Sender;
        private Person Reciever;
        private MimeMessage message;

        public Email(Person sender, Person reciever)
        {
            this.Sender = sender;
            this.Reciever = reciever;
            this.message = new MimeMessage();
            this.SetUpMessageTo_From();
        }

        private void SetUpMessageTo_From()
        {
            message.From.Add(new MailboxAddress("Admin", "ScheduleBuilder2019@gmail.com"));
        
            message.To.Add(new MailboxAddress(Reciever.GetFullName(), Reciever.Email));
        }

        public void SendMessage(string subject, string body)
        {
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("ScheduleBuilder2019@gmail.com", "!Yoder19");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}