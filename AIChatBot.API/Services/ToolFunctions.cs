using AIChatBot.API.Models.Custom;

namespace AIChatBot.API.Services
{
    public static class ToolFunctions
    {
        public static string CreateFile(string filename, string content)
        {
            var path = Path.Combine("AgentFiles", filename);
            Directory.CreateDirectory("AgentFiles");
            File.WriteAllText(path, content);
            return $"✅ File '{filename}' created.";
        }

        public static string FetchWebData(string url)
        {
            using var client = new HttpClient();
            var result = client.GetStringAsync(url).Result;
            //Call AI model to get the summary from the result object.
            return $"🌐 Data from {url.Substring(0, Math.Min(url.Length, 50))}...:\n{result.Substring(0, Math.Min(url.Length, 200))}";
        }

        public static string SendEmail(string to, string subject, string body)
        {
            // Build MailData
            var mailData = new MailData
            {
                EmailToId = to,
                EmailToName = to, // Or set as needed
                EmailSubject = subject,
                EmailBody = body
            };

            // Get the service provider from a static accessor
            var provider = ServiceProviderAccessor.ServiceProvider;
            if (provider == null)
                return "❌ MailService not available.";

            var mailService = provider.GetService<MailService>();
            if (mailService == null)
                return "❌ MailService not available.";

            var result = mailService.SendMail(mailData);
            return result ? $"📧 Email sent to {to} with subject '{subject}'." : "❌ Failed to send email.";
        }
    }

    // Static accessor for the root IServiceProvider
    public static class ServiceProviderAccessor
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
