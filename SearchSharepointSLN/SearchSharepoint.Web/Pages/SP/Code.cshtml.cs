using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using SearchSharepoint.Web.Models;
using SearchSharepoint.Web.Services;

namespace SearchSharepoint.Web.Pages.SP;

public class CodePageModel(ILogger<CodePageModel> logger, ISharepointSearchServices sharepointSearchServices)
    : PageModel
{
    public async Task OnGetAsync()
    {
        logger.LogInformation("Code Index page visited at {DateLoaded} with Code {Code}", DateTime.UtcNow, Code);
        if (!string.IsNullOrEmpty(Query))
        {
            logger.LogInformation("Calling Sharepoint search with query {Query}", Query);
            SearchResults = await sharepointSearchServices.SearchWithAsync(Code, Query);
            logger.LogInformation("Search results returned {SearchResultsCount}", SearchResults.Count);
        }
    }

    [BindProperty(SupportsGet = true)] public string Query { get; set; } = "";
    [BindProperty(SupportsGet = true)] public string Code { get; set; }
    [BindProperty] public List<SearchModel> SearchResults { get; set; } = new();
}