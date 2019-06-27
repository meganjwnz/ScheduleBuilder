using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleBuilder.Model
{
    
    public class EmailAlteredPerson : Email
    {
        private Person Sender;
        private Person Reciever;

        public EmailAlteredPerson(Person sender, Person reciever) : base(sender, reciever)
        {
            this.Sender = sender;
            this.Reciever = reciever;
        }
        protected override string SetUpSubject()
        {
            return $"Hello {this.Reciever.GetFullName() } your personal information has changed";
        }

        protected override string SetUpBody()
        {
            
            return $"Hello { Reciever.GetFullName()}, You are recieving this email to let you know that { Sender.GetFullName() } Has altered your information" +
            $" " +
            $"If this has been done in error please contact your Admin as soon as possible ";
        }
    }
}