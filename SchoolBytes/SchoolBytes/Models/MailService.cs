using System.Net.Mail;

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

        public void send(string from, string to) {
            mail = new MailMessage(from, to);
            SmtpClient.Send(mail);
        }

        // dummy
        public void ClassCanceledNotification(CourseModule cm)
        {

            // send emails til alle cm.registrations.participants med en predefineret email
            // Noget i stil med "Hej participant.name, hold undervisningen cm.name for cm.course.name er desværre 
            //  aflyst.   /Kompetencehuset

        }
    }
}