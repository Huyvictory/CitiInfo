namespace CitiInfo.WebAPI.Services
{
    public class LocalMailService
    {
        private string _mailto = "admin@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            // send mail - log output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailto}, with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
