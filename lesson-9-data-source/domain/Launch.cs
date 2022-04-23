
using System.Collections.Immutable;

namespace Domain
{
    public class Launch
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly DateTime Date;
        public readonly ImageUrl Image;
        public readonly ImmutableList<ImageUrl> Images;
        public readonly string VideoUrl;
        public readonly string ArticleUrl;

        public Launch(
            string id, 
            string name, 
            string description, 
            DateTime date, 
            ImageUrl image, 
            ICollection<ImageUrl> images,
            string videoUrl,
            string articleUrl
            ) {
                this.Id = id;
                this.Name = name;
                this.Description = description;
                this.Date = date;
                this.Image = image;
                this.Images = images.ToImmutableList();
                this.VideoUrl = videoUrl;
                this.ArticleUrl = articleUrl;
        }

        public override string ToString()
        {
            return $"Launch({Id}, {Name}, {Description}, {Date}, {Image}, {VideoUrl}, {ArticleUrl})";
        }
    }
}