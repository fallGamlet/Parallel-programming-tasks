using Domain;

namespace SpacexApi
{
    public class LaunchMapper
    {
        public List<Launch> MapLaunches(List<LaunchJson> jsonList) {
            return jsonList
                .Select(MapLaunch)
                .ToList();
        }

        public Launch MapLaunch(LaunchJson json) 
        {
            return new Launch(
                json.id ?? "",
                json.name ?? "",
                json.details ?? "",
                json.date_local ?? DateTime.MinValue,
                MapImageUrl(json.links?.patch),
                MapImages(json.links?.flickr),
                json.links?.webcast ?? "",
                json.links?.wikipedia ?? ""
            );
        }

        private ImageUrl MapImageUrl(PatchJson? json)
        {
            if (json == null) return ImageUrl.EMPTY;

            return new ImageUrl(
                json?.small ?? "",
                json?.large ?? ""
            );
        }

        private List<ImageUrl> MapImages(FlickrJson? json)
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

    }
}