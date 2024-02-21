using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SearchSharepoint.Web.Models;
using SearchSharepoint.Web.Services;

namespace SearchSharepoint.Web.Pages.SP;

public class CodePageModel(ILogger<CodePageModel> logger, ISharepointSearchServices sharepointSearchServices)
    : PageModel
{
    public void OnGet()
    {
        logger.LogInformation("Code Index page visited at {DateLoaded} with Code {Code}", DateTime.UtcNow, Code);
        if (TempData["SearchResults"] != null) 
            SearchResults = TempData["SearchResults"] as List<SearchModel> ?? [];
    }

    public async Task<IActionResult> OnPostAsync()
    {
        logger.LogInformation("Performing search with query {Query}", Query);
        if (!string.IsNullOrEmpty(Query))
        {
            logger.LogInformation("Calling Sharepoint search with query {Query}", Query);
            SearchResults = await sharepointSearchServices.SearchWithAsync(Code, Query);
            logger.LogInformation("Search results returned {SearchResultsCount}", SearchResults.Count);
            TempData["SearchResults"] = SearchResults;
        }

        return RedirectToPage("/SP/Code", new { code = Code });
    }

    [BindProperty] public string Query { get; set; } = "";
    [BindProperty(SupportsGet = true)] public string Code { get; set; }
    [BindProperty] public List<SearchModel> SearchResults { get; set; } = [];
}