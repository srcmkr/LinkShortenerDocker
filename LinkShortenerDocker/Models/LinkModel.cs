using LiteDB;

namespace LinkShortenerDocker.Models
{
    public class LinkModel
    {
        [BsonId]
        public int Id { get; set; }

        public string RedirectTargetUrl { get; set; }
    }
}
