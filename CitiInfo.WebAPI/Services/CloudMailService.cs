namespace CitiInfo.WebAPI.Services
{
    public class CloudMailService : IMailService
    {
        private readonly string _mailto = string.Empty;
        private readonly string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailto = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            // send mail - log output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailto}, with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
