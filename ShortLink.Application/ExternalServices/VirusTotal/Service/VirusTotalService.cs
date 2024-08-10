using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ShortLink.Application.ExternalServices.VirusTotal.Interface;
using ShortLink.Infra.Interfaces;

namespace ShortLink.Application.ExternalServices.VirusTotal.Service
{
    public class VirusTotalService : IVirusTotalService
    {
        private static readonly HttpClient client = new();
        private static string apiUrl;
        private static string analysisUrl;
        private static string? virusTotalApiKey;

        public VirusTotalService(IConfiguration configuration)
        {
            apiUrl = "https://www.virustotal.com/api/v3/urls";
            virusTotalApiKey = configuration["VirusTotalApiKey"];
            analysisUrl = $"https://www.virustotal.com/api/v3/analyses/";
            client.DefaultRequestHeaders.Add("x-apikey", virusTotalApiKey);

        }


        public async Task<bool> SendRlForAnalysis(string url)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("url",url)
            });
            var response = await client.PostAsync(apiUrl, formContent);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject resultBody = JObject.Parse(jsonResponse);
            return await CheckIfUrlIsSafe(resultBody["Data"]["id"].ToString());
        }
        private static async Task<bool> CheckIfUrlIsSafe(string id)
        {
            HttpResponseMessage response = await client.GetAsync(analysisUrl + id);
            string jsonResponse = await response.Content.ReadAsStringAsync();
            JObject jsonObject = JObject.Parse(jsonResponse);
            var stats = jsonObject["Data"]["attributes"]["stats"];

            int malicious = (int)stats["malicious"];
            int suspicious = (int)stats["suspicious"];
            int harmless = (int)stats["harmless"];


            return SafeTermometer(malicious, suspicious, harmless);



        }
        private static bool SafeTermometer(int malicious, int suspicious, int harmless)
        {
            int total = malicious + suspicious + harmless;
            int unSafe = (malicious + suspicious / total) * 100;
            int safe = harmless / total * 100;

            return safe > unSafe;
        }

    }
}
