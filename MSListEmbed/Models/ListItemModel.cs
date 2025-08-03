namespace MSListEmbed.Models
{
    public class ListItemModel
    {
        public string? Title { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? HostingDatesLength { get; set; }
        public string? LocationDetails { get; set; }
        public string? ImportantDetails { get; set; }
        public int DisplayOrder { get; set; }

        public string GetLink(string linkTemplate)
        {
            if (string.IsNullOrEmpty(linkTemplate)) return string.Empty;
            return linkTemplate
                .Replace("__STATUS__", Status ?? string.Empty)
                .Replace("__TITLE__", Title ?? string.Empty);
        }
    }
}
