using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Web;

namespace SchoolBytes.Models
{
    public class MailService
    {
        private MailMessage mail;
        private readonly SmtpClient SmtpClient;

        public MailService(string host)
        {
            SmtpClient = new SmtpClient(host, 25);
            SmtpClient.EnableSsl = true;
        }

        public MailService(string host, int port)
        {
            SmtpClient = new SmtpClient(host, port);
            SmtpClient.EnableSsl = true;
        }

        public MailService(string host, int port, bool enableSSL)
        {
            SmtpClient = new SmtpClient(host, port);
            SmtpClient.EnableSsl = enableSSL;
        }

        public void setContent(string subject, string body)
        {
            mail.Subject = subject;
            mail.Body = body;
        }

        public void setAttachment(string path)
        {
            Attachment attachment = new Attachment(path);
            mail.Attachments.Add(attachment);
        }

        public void setCredentials(string username, string password)
        {
            SmtpClient.Credentials = new System.Net.NetworkCredential(username, password);
        }

        public void setCredentialsfromConfig()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/EmailCredentials.json");
            StreamReader credJson = new StreamReader(filePath);
            string[] cred = ((string)JObject.Parse(credJson.ReadToEnd())["credentials"]).Split(" ".ToCharArray());
            string username = cred[0];
            string password = cred[1];
            SmtpClient.Credentials = new System.Net.NetworkCredential(username, password);
        }

        public void send(string from, string to) {
            mail = new MailMessage(from, to);
            SmtpClient.Send(mail);
        }


        public void ClassCanceledNotification(CourseModule cm)
        {
            setCredentialsfromConfig();
            mail.Subject = "Class Canceled";
            foreach (var registration in cm.Registrations)
            {
                var participant = registration.participant;
                if (participant.Email == null || participant.Email.Length == 0) continue;
                mail.Body = $"Dear {participant.Name},\n\nWe regret to inform you that the class {cm.Name} has been canceled.\n\nWe apologize for any inconvenience this may have caused.\n\nSincerely,\n\nThe SchoolBytes Team";
                send("notifications.SchoolBytes.com", participant.Email);
            }
        }
    }
}