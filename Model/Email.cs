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
            this.SetUpMessageTo_From_Subject_Body();
        }

        private void SetUpMessageTo_From_Subject_Body()
        {
            message = new MimeMessage();

            message.From.Add(new MailboxAddress("Admin", "ScheduleBuilder2019@gmail.com"));
        
            message.To.Add(new MailboxAddress(Reciever.GetFullName(), Reciever.Email));

           // string subject = this.SetUpSubject();

            message.Subject =" subject";// this.SetUpSubject();

            message.Body = new TextPart("plain")
            {
                Text = "test body "//this.SetUpBody()
            };
        }


        protected virtual string SetUpSubject()
        {
            return "Notification from ScheduleBuilder";
        }

        protected virtual string SetUpBody()
        {
        return $"Hello { this.Reciever.GetFullName()}, You are recieving this email to let you know that { Sender.GetFullName() } Has tried to message you";
        }

        public void SendMessage()
        {
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