using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SearchSharepoint.Web.Models;
using SearchSharepoint.Web.Services;

namespace SearchSharepoint.Web.Pages.PWD;

public class IndexPageModel(
    ILogger<IndexPageModel> logger,
    ISharepointSearchServices sharepointSearchServices) : PageModel
{
    public void OnGet()
    {
        if (Request.QueryString.HasValue) Code = Request.Query["code"].ToString();

        logger.LogInformation("Code Index page visited at {DateLoaded} with Code {Code}", DateTime.UtcNow, Code);
        if (TempData["SearchResults"] != null)
        {
            var data = TempData["SearchResults"]?.ToString();
            logger.LogInformation("Search results found in TempData {Data}", data);
            if (!string.IsNullOrEmpty(data)) 
                SearchResults = JsonConvert.DeserializeObject<List<SearchModel>>(data) ?? [];
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        logger.LogInformation("Performing search with query {Query}", Query);
        if (!string.IsNullOrEmpty(Query))
        {
            logger.LogInformation("Calling Sharepoint search with query {Query} and {Code}", Query, Code);
            SearchResults = await sharepointSearchServices.SearchWithAsync(Code, Query);
            logger.LogInformation("Search results returned {SearchResultsCount}", SearchResults.Count);
            TempData["SearchResults"] = JsonConvert.SerializeObject(SearchResults);
        }

        return RedirectToPage("/PWD/Index", new { code = Code });
    }

    [BindProperty] public string Query { get; set; }
    [BindProperty] public string Code { get; set; }
    [BindProperty] public List<SearchModel> SearchResults { get; set; } = [];
}