using System.Net.Http.Headers;
using System.Text.Json;
using DataJson;
using Domain;

namespace Data
{
    class LaunchDataSourceImpl : ILaunchDataSource
    {
        private readonly HttpClient client = new HttpClient();
        private Dictionary<string, Launch> launchDict = new Dictionary<string, Launch>();

        public Task<List<Launch>> getLaunches()
        {
            return getLauncesRemote();
        }

        private Task<List<Launch>> getLauncesRemote() {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            return client.GetStringAsync("https://api.spacexdata.com/v4/launches")
            .ContinueWith<List<Launch>>(task => 
            {
                var textJson = task.Result;
                var launchListJson = JsonSerializer.Deserialize<List<LaunchJson>>(textJson);
                List<Launch> launchList = launchListJson
                    ?.Select(mapLaunch)
                    ?.Take(5)
                    ?.ToList() 
                    ?? new List<Launch>();

                launchDict.Clear();
                launchList.ForEach(item => 
                {
                    launchDict[item.Id] = item;
                });

                return launchList;
            });
        }

        private Launch mapLaunch(LaunchJson json) 
        {
            return new Launch(
                json.id ?? "",
                json.name ?? "",
                json.details ?? "",
                json.date_local ?? DateTime.MinValue,
                mapImageUrl(json.links?.patch),
                mapImages(json.links?.flickr),
                json.links?.webcast ?? "",
                json.links?.wikipedia ?? ""
            );
        }

        private ImageUrl mapImageUrl(PatchJson? json)
        {
            if (json == null) return ImageUrl.EMPTY;

            return new ImageUrl(
                json?.small ?? "",
                json?.large ?? ""
            );
        }

        private List<ImageUrl> mapImages(FlickrJson? json)
        {
            if (json == null) return new List<ImageUrl>();

            var smallCount = json.small?.Count ?? 0;
            var largeCount = json.original?.Count ?? 0;
            if (smallCount == 0 && largeCount == 0) return new List<ImageUrl>();

            var resultList = new List<ImageUrl>(Math.Max(smallCount, largeCount));
            for (int i=0; i < smallCount || i < largeCount; i++) {
                string? small = null;
                string? large = null;
                if (i < smallCount) small = json.small?[i];
                if (i < largeCount) small = json.original?[i];

                resultList.Add(
                    new ImageUrl(
                        small ?? "",
                        large ?? ""
                    )
                );
            }
            return resultList;
        }

        public Task<Launch> getLaunch(string launchId)
        {
            return Task<Launch>.Run(() => 
            {
                try {
                    return launchDict[launchId];
                } catch(Exception e) {
                    throw new KeyNotFoundException($"launch with Id {launchId} not found", e);
                }
            });
        }
    }

    
}