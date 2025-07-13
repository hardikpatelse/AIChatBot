using AIChatBot.API.Models.Custom;
using HtmlAgilityPack;
using System.Text;

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
            var html = client.GetStringAsync(url).Result;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Attempt to extract main readable content: paragraphs and headlines
            var nodes = doc.DocumentNode.SelectNodes("//h1 | //h2 | //h3 | //p");

            if (nodes == null || nodes.Count == 0)
                return $"🌐 No readable content found at {url}";

            var sb = new StringBuilder();
            foreach (var node in nodes)
            {
                var text = HtmlEntity.DeEntitize(node.InnerText).Trim();
                if (!string.IsNullOrEmpty(text) && text.Length > 40) // filter noise
                {
                    sb.AppendLine($"• {text}");
                }
            }

            // Truncate for preview if very long
            var preview = sb.ToString();
            if (preview.Length > 1000)
                preview = preview.Substring(0, 1000) + "...";

            return $"🌐 Extracted summary from {url}:\n\n{preview}";
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
