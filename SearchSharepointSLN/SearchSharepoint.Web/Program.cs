using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using SearchSharepoint.Web.Options;
using SearchSharepoint.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<AzureAdOptions>()
    .Bind(builder.Configuration.GetSection(OptionNames.AzureAdOptionsName))
    .ValidateDataAnnotations();
builder.Services.AddOptions<SharepointOptions>()
    .Bind(builder.Configuration.GetSection(OptionNames.SharepointOptionsName))
    .ValidateDataAnnotations();

builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ISharepointSearchServices, SharepointSearchService>();

builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection(OptionNames.AzureAdOptionsName));

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
        options.Conventions.AddPageRoute("/Info/Index", ""))
    .AddMvcOptions(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    }).AddMicrosoftIdentityUI();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Error");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health").AllowAnonymous();
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});
app.Run();