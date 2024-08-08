namespace ShortLink.Domain
{
    public class Link
    {
        public Guid _Id { get; set; }
        public string? Url { get; set; }
        public string? Hash { get; set; }
        public bool Active { get; set; }


    }
}