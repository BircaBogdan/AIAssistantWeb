using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AIAssistant.Core.Services
{
    public class OllamaService
    {
        private readonly HttpClient _httpClient;

        public OllamaService()
        {
            _httpClient = new HttpClient();
        }

        public async IAsyncEnumerable<string> StreamResponseAsync(string prompt, double temperature)
        {
            var requestBody = new
            {
                model = "gemma3:1b",
                prompt = prompt,
                stream = true,
                options = new
                {
                    temperature = temperature
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:11434/api/generate")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead);

            var stream = await response.Content.ReadAsStreamAsync();
            var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var json = JsonDocument.Parse(line);

                if (json.RootElement.TryGetProperty("response", out var token))
                {
                    yield return token.GetString();
                }
            }
        }
    }
}