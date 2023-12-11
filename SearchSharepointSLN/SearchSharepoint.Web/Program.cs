using SearchSharepoint.Web.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<WebOptions>()
    .Bind(builder.Configuration.GetSection(OptionNames.WebOptionsName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
    options.Conventions.AddPageRoute("/Info/Index", ""));

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