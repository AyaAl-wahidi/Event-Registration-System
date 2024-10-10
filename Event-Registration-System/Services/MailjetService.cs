using Event_Registration_System.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace Event_Registration_System.Services
{
    public class MailjetService
    {
        private readonly IConfiguration _configuration;
        private readonly MailjetClient _mailClient;

        public MailjetService(IConfiguration configuration, MailjetClient mailjetClient)
        {
            _configuration = configuration;
            _mailClient = new MailjetClient
                (
                _configuration["Mailjet:ApiKey"],
                _configuration["Mailjet:SecretKey"]
                );
        }

        public async Task<bool> SendEmail(string participantName, string email, string content)
        {
            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "ayanalwahidi@gmail.com")
            .Property(Send.FromName, "Aya")
            .Property(Send.Subject, "Event Registration")
            .Property(Send.HtmlPart, content)
            .Property(Send.Recipients, new JArray {
            new JObject {
                {"Email", email}
            }
            });

            MailjetResponse response = await _mailClient.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}