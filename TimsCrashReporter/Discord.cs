using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace TimsCrashReporter
{
    class Discord
    {
        static string s_WebhookUrl = string.Empty;

        private static readonly HttpClient s_HttpClient = new HttpClient();

        private static readonly string s_MultipartBoundary = "SomeContentBoundary";

        public static async Task SendToDiscord(CrashInfo a_CrashInfo, string a_CrashDescription)
        {
            try
            {
                var content = new MultipartFormDataContent(s_MultipartBoundary);

                ByteArrayContent crashContent = new ByteArrayContent(a_CrashInfo.ZipCrashInfo());
                string fileName = "Crash-" + DateTime.UtcNow.ToString("yyyy-MM-dd--HH-mm") + ".zip";
                content.Add(crashContent, fileName, fileName);
                
                var embedStr = "{\"embeds\": " +
                    "[" +
                        "{" +
                            "\"title\": \"Crash Report\"," +
                            "\"description\": \"" + a_CrashDescription + "\"," +
                            "\"color\": \"16745472\"" +
                        "}" +
                    "]" +
                "}";
                content.Add(new StringContent(embedStr), "payload_json");

                await s_HttpClient.PostAsync(s_WebhookUrl, content);
            }
            catch(HttpRequestException e)
            {
                //Console.WriteLine(e.Message);
            }
        }
    }
}
