using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SearchSharepoint.Web.Models;
using SearchSharepoint.Web.Services;

namespace SearchSharepoint.Web.Pages.SP;

public class IndexPageModel(ILogger<IndexPageModel> logger, 
    ISharepointSearchServices sharepointSearchServices) : PageModel
{
    public async Task OnGetAsync()
    {
        logger.LogInformation("Index page visited at {DateLoaded} with Query {Query}", DateTime.UtcNow, Query);
        if (!string.IsNullOrEmpty(Query))
        {
            logger.LogInformation("Calling Sharepoint search with query {Query}", Query);
            SearchResults = await sharepointSearchServices.SearchAsync(Query);
            logger.LogInformation("Search results returned {SearchResultsCount}", SearchResults.Count);
        }
    }

    [BindProperty(SupportsGet = true)] public string Query { get; set; } = "";
    [BindProperty] public List<SearchModel> SearchResults { get; set; } = new();
}