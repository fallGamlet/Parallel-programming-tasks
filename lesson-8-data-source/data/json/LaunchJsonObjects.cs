namespace DataJson
{

    public class LaunchJson
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? details { get; set; }
        public DateTime? date_utc { get; set; }
        public int? date_unix { get; set; }
        public DateTime? date_local { get; set; }
        public LinksJson? links { get; set; }
        public DateTime? static_fire_date_utc { get; set; }
        public int? static_fire_date_unix { get; set; }
        public bool? net { get; set; }
        public int? window { get; set; }
        public string? rocket { get; set; }
        public bool? success { get; set; }
        public List<string>? crew { get; set; }
        public List<string>? ships { get; set; }
        public List<string>? capsules { get; set; }
        public List<string>? payloads { get; set; }
        public string? launchpad { get; set; }
        public int? flight_number { get; set; }
        public string? date_precision { get; set; }
        public bool? upcoming { get; set; }
        public List<CoreJson> cores { get; set; }
        public bool? auto_update { get; set; }
        public bool? tbd { get; set; }
        public string? launch_library_id { get; set; }
    }

    public class LinksJson
    {
        public PatchJson? patch { get; set; }
        public RedditJson? reddit { get; set; }
        public FlickrJson? flickr { get; set; }
        public string? presskit { get; set; }
        public string? webcast { get; set; }
        public string? youtube_id { get; set; }
        public string? article { get; set; }
        public string? wikipedia { get; set; }
    }
    
    public class PatchJson
    {
        public string? small { get; set; }
        public string? large { get; set; }
    }

    public class RedditJson
    {
        public string? campaign { get; set; }
        public string? launch { get; set; }
        public string? media { get; set; }
        public string? recovery { get; set; }
    }

    public class FlickrJson
    {
        public List<string>? small { get; set; }
        public List<string>? original { get; set; }
    }

    public class CoreJson
    {
        public string? core { get; set; }
        public int? flight { get; set; }
        public bool? gridfins { get; set; }
        public bool? legs { get; set; }
        public bool? reused { get; set; }
        public bool? landing_attempt { get; set; }
        public bool? landing_success { get; set; }
        public string? landing_type { get; set; }
        public string? landpad { get; set; }
    }

}