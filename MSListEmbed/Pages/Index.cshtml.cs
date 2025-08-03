using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Graph;
using MSListEmbed.Models;

namespace MSListEmbed.Pages;

public class IndexModel : PageModel
{
    private readonly GraphServiceClient _graphClient;
    private readonly IConfiguration _config;

    public List<ListItemModel> ListItems { get; set; } = new();

    public string LinkTemplate => _config["LinkTemplate"] ?? "";

    public IndexModel(GraphServiceClient graphClient, IConfiguration config)
    {
        _graphClient = graphClient;
        _config = config;
    }

    public async Task OnGetAsync()
    {
        var siteId = _config["Graph:SiteId"];
        var listId = _config["Graph:ListId"];
        if (string.IsNullOrEmpty(siteId) || string.IsNullOrEmpty(listId))
            return;

        var items = await _graphClient.Sites[siteId].Lists[listId].Items
            .GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Expand = ["fields"];
            });

        ListItems = items?.Value?.Select(i => new ListItemModel
        {
            Title = i.Fields!.AdditionalData.TryGetValue("Title", out var title) ? title?.ToString() : null,
            Status = i.Fields!.AdditionalData.TryGetValue("board_x0020_choice", out var status) ? status?.ToString() : null,
            Description = i.Fields!.AdditionalData.TryGetValue("Description", out var desc) ? desc?.ToString() : null,
            HostingDatesLength = i.Fields!.AdditionalData.TryGetValue("Hosting_x0020_dates_x002f_length", out var dates) ? dates?.ToString() : null,
            LocationDetails = i.Fields!.AdditionalData.TryGetValue("Location_x0020_details_x003a_", out var loc) ? loc?.ToString() : null,
            ImportantDetails = i.Fields!.AdditionalData.TryGetValue("Important_x0020_Details_x003a_", out var important) ? important?.ToString() : null,
            DisplayOrder = i.Fields!.AdditionalData.TryGetValue("DisplayOrder", out var order) ? order as decimal? ?? 0 : 0,
        }).ToList() ?? [];
    }
}
