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
            // Simulate email sending (log to console)
            return $"📧 Email sent to {to} with subject '{subject}' \n Body: {body}.";
        }
    }

}
