using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchSharepoint.Web.Pages.Info;

public class IndexModel(ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> logger = logger;

    public void OnGet() => logger.LogInformation("Info page visited at {DateLoaded}.", DateTime.UtcNow);
}