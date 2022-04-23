using System.Net.Http.Headers;
using System.Text.Json;

namespace SpacexApi
{
    public class SpacexApiImpl : ISpacexApi
    {
        private readonly HttpClient client;

        public SpacexApiImpl()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "MySpacexApp");
        }
        public Task<List<LaunchJson>> GetLaunches()
        {
            return client.GetStringAsync("https://api.spacexdata.com/v4/launches")
            .ContinueWith<List<LaunchJson>>(task => 
            {
                List<LaunchJson>? launchListJson = null;
                var textJson = task.Result;
                launchListJson = JsonSerializer.Deserialize<List<LaunchJson>>(textJson);
                return launchListJson ?? new List<LaunchJson>();
            });
        }
    }
}