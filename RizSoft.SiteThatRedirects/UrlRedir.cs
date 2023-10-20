namespace RizSoft.SiteThatRedirects
{
    public record UrlRedir
    {
        public string? Path { get; set; }
        public string? NewUrl { get; set; }
    }
}
