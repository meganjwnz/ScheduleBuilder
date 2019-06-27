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
        MimeMessage message = new MimeMessage();

        public Email(Person addressie)
        {
            message.To.Add(new MailboxAddress(addressie.GetFullName(), addressie.Email));
        }

        public Email(Person sender, string email, string fullname)
        {
            message.To.Add(new MailboxAddress(fullname, email));
        }


        public void SendMessage(string subject, string body)
        {
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            message.From.Add(new MailboxAddress("Admin", "ScheduleBuilder2019@gmail.com"));

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