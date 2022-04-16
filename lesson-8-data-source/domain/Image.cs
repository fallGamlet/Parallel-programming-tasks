
namespace Domain
{
    class ImageUrl
    {
        public readonly string Small;
        public readonly string Large;

        public static ImageUrl EMPTY = new ImageUrl("", "");

        public ImageUrl(string small, string large) {
            this.Small = small;
            this.Large = large;
        }

        public override string ToString()
        {
            return $"ImageUrl(small=\"{Small}\", large=\"{Large}\")";
        }
    }
}