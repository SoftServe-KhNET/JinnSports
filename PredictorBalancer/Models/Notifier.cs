using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace PredictorBalancer.Models
{
    public class Notifier
    {
        public void SendEmail(string message)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("rataichuktestapp@gmail.com");
            mail.To.Add("rockarolla6666@gmail.com");
            mail.Subject = "PredictionApp Notification";
            mail.Body = message;

            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("rataichuktestapp@gmail.com", "air051088");
            smtpServer.EnableSsl = true;

            smtpServer.Send(mail);
        }
    }
}