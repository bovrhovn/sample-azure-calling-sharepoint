﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SearchSharepoint.Web.Pages.SP;

public class IndexPageModel(ILogger<IndexPageModel> logger) : PageModel
{
    public void OnGet()
    {
        logger.LogInformation("Index page visited at {DateLoaded}", DateTime.UtcNow);
    }

    [BindProperty(SupportsGet = true)]
    public string Query { get; set; }
}