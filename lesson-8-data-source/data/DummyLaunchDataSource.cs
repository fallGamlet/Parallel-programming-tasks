using System.Collections.Immutable;
using Domain;

namespace Data
{
    class DummyLaunchDataSource : ILaunchDataSource
    {
        private List<Launch> launches;
        public DummyLaunchDataSource() {
            launches = new List<Launch> {
                new Launch(
                    "l-1",
                    "Launch 1",
                    "Launch 1 description",
                    DateTime.Now.AddDays(-25),
                    ImageUrl.EMPTY,
                    new List<ImageUrl>(),
                    "https://video-url",
                    "https://article-url"
                ),
                new Launch(
                    "l-2",
                    "Launch 2",
                    "Launch 2 description",
                    DateTime.Now.AddDays(-65),
                    ImageUrl.EMPTY,
                    new List<ImageUrl>(),
                    "https://video-url",
                    "https://article-url"
                ),
                new Launch(
                    "l-3",
                    "Launch 3",
                    "Launch 3 description",
                    DateTime.Now.AddDays(-32),
                    ImageUrl.EMPTY,
                    new List<ImageUrl>(),
                    "https://video-url",
                    "https://article-url"
                ),
                new Launch(
                    "l-4",
                    "Launch 4",
                    "Launch 4 description",
                    DateTime.Now.AddDays(-163),
                    ImageUrl.EMPTY,
                    new List<ImageUrl>(),
                    "https://video-url",
                    "https://article-url"
                ),
                new Launch(
                    "l-5",
                    "Launch 5",
                    "Launch 5 description",
                    DateTime.Now.AddDays(-321),
                    ImageUrl.EMPTY,
                    new List<ImageUrl>(),
                    "https://video-url",
                    "https://article-url"
                )
            };
        }

        public Task<List<Launch>> getLaunches()
        {
            return Task.Run(() => new List<Launch>(launches));
        }

        public Task<Launch> getLaunch(string launchId)
        {
            return Task.Run(() => 
            {
                var launch = launches.Find((item) => item.Id == launchId);
                if (launch == null) {
                    throw new KeyNotFoundException($"launch with Id {launchId} not found");
                }
                return launch;
            });
        }

        
    }
}