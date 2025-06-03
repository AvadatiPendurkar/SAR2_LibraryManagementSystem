using System.Net.Mail;
using System.Net;

namespace SAR2_LibraryManagementSystem
{
    public class EmailService
    {
        private readonly IConfiguration Configuration;
        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = Configuration.GetSection("Constants:FromEmail").Value ?? string.Empty;
            var fromEmailPassword = Configuration.GetSection("Constants:EmailAccountPassword").Value ?? string.Empty;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromEmailPassword),
                EnableSsl = true,
            };

            var message = new MailMessage()
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

           await  smtpClient.SendMailAsync(message);
        }
    }
}
