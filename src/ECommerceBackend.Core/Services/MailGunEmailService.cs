using System;
using System.Threading.Tasks;
using ECommerceBackend.Core.Interfaces;
using ECommerceBackend.Core.UserSecrets;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace ECommerceBackend.Core.Services
{
    public class MailGunEmailService : IEmailService
    {
        protected readonly IConfiguration _configuration;
        public MailGunEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                string apiKey = Environment.GetEnvironmentVariable("ECommerce_MAILGUN_API_KEY");
                string domain = Environment.GetEnvironmentVariable("ECommerce_MAILGUN_DOMAIN");

                if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(domain))
                {
                    MailGun mailGun = _configuration.GetSection("MailGun").Get<MailGun>();
                    if (mailGun != null)
                    {
                        apiKey = mailGun.ApiKey;
                        domain = mailGun.Domain;
                    }
                }

                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", apiKey);
                RestRequest request = new RestRequest();
                request.AddParameter("domain", domain, ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", $"BET Shop <info@{domain}>");
                request.AddParameter("to", to);
                request.AddParameter("bcc", "dev.denniskay@outlook.com");
                request.AddParameter("subject", subject);
                request.AddParameter("html", body);
                request.Method = Method.POST;

                var response = await client.ExecuteAsync(request);
            }
            catch (Exception)
            {

            }
        }
    }
}
