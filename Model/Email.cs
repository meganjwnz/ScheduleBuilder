using MailKit.Net.Smtp;
using MimeKit;

namespace ScheduleBuilder.Model
{
    public class Email
    {
        MimeMessage message = new MimeMessage();

        /// <summary>
        /// Email constructor - the person who will be email will be sent too
        /// </summary>
        /// <param name="addressie"></param>
        public Email(Person addressie)
        {
            message.To.Add(new MailboxAddress(addressie.GetFullName(), addressie.Email));
        }

        /// <summary>
        /// Builds message using accepted name and email
        /// </summary>
        /// <param name="fullname"></param>
        /// <param name="email"></param>
        public Email(string fullname, string email)
        {
            message.To.Add(new MailboxAddress(fullname, email));
        }

        /// <summary>
        /// Sends an email with the subject and body
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
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