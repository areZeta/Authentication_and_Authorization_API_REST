using System.Net.Mail;
using System.Net;

namespace IdentityAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration Configuration;

        public EmailService(IConfiguration config) => Configuration = config;

        public (int StatusCode, string Message, MailMessage email) ConfigurationEmail(string to, string subject, string body)
        {
            MailMessage? Email = new MailMessage();
            try
            {
                if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(subject)  || string.IsNullOrEmpty(body))
                    return (StatusCodes.Status400BadRequest, "Require data.", Email);
                var from = Configuration.GetSection("EmailService:From").Value;
                var host = Configuration.GetSection("EmailService:Host").Value;
                var port = Configuration.GetSection("EmailService:Port").Value;
                var password = Configuration.GetSection("EmailService:Password").Value;
                var displayName = Configuration.GetSection("EmailService:UserName").Value; ;

                Email.From = new MailAddress(from!, displayName);
                Email.To.Add(to); 
                Email.Subject = subject;
                Email.Body = body; 
                Email.IsBodyHtml = true;
                Email.Priority = MailPriority.High;
                return (StatusCodes.Status200OK, "Successful", Email);
            }
            catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message, Email);
            }
        }

        public (int StatusCode, string Message) SendEmail(MailMessage email)
        {
            try
            {
                var from = Configuration.GetSection("EmailService:From").Value;
                var host = Configuration.GetSection("EmailService:Host").Value;
                var port = Configuration.GetSection("EmailService:Port").Value;
                var password = Configuration.GetSection("EmailService:Password").Value;

                SmtpClient client = new (host, int.Parse(port!));
                client.Credentials = new NetworkCredential(from, password);
                client.EnableSsl = true;
                client.Send(email);
                return (StatusCodes.Status200OK, "Successful send email");
            }catch (Exception ex)
            {
                return (StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
