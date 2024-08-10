namespace ShortLink.Domain
{
    public class Link
    {
        public Guid UserId { get; set; }
        public string? Url { get; set; }
        public string? Hash { get; set; }
        public bool Active { get; set; }
        public int AccesCount { get; set; }

    }
}