using System.Net;
using System.Net.Mail;

namespace SonaBookings.Models
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("2100003779@nttu.edu.vn","jcfg pkhg btdu tltb"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("2100003779@nttu.edu.vn"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);
            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




        }
    }
}
