using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            var requestBody = new
            {
                model = "gemma3:1b",
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(
                "http://localhost:11434/api/generate",
                content);

            if (!response.IsSuccessStatusCode)
            {
                return $"Ollama error: {response.StatusCode}";
            }

            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(json);

            return doc.RootElement
                .GetProperty("response")
                .GetString();
        }
    }
}
