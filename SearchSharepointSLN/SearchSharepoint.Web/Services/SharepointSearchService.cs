using System.Net;
using System.Net.Http.Headers;
using System.Security;
using System.Web;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using SearchSharepoint.Web.Models;
using SearchSharepoint.Web.Options;

namespace SearchSharepoint.Web.Services;

public class SharepointSearchService : ISharepointSearchServices
{
    private readonly HttpClient httpClient;
    private readonly ILogger<SharepointSearchService> logger;
    private readonly SharepointOptions sharepointOptions;
    private readonly AzureAdOptions azureAdOptions;

    public SharepointSearchService(HttpClient httpClient,
        IOptions<AzureAdOptions> azureAdOptions,
        IOptions<SharepointOptions> sharepointOptionsValue,
        ILogger<SharepointSearchService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
        sharepointOptions = sharepointOptionsValue.Value;
        httpClient.BaseAddress = new Uri(sharepointOptions.TenantUrl, UriKind.Absolute);
        this.azureAdOptions = azureAdOptions.Value;
    }

    public async Task<string> LoginAsync()
    {
        var app = ConfidentialClientApplicationBuilder.Create(azureAdOptions.ClientId)
            .WithClientSecret(azureAdOptions.Secret)
            .WithTenantId(azureAdOptions.TenantId)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{azureAdOptions.TenantId}"))
            .WithLegacyCacheCompatibility(false)
            .Build();
        var scopes = new[] { $"{sharepointOptions.TenantUrl}.default" };
        var token = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        return token == null ? string.Empty : token.AccessToken;
    }

    public async Task<List<SearchModel>> SearchAsync(string query)
    {
        logger.LogInformation("Logging to Azure Application.");
        var bootstrapToken = await LoginAsync();
        logger.LogInformation("Bootstrap token received. {BoostrapToken}", bootstrapToken);
        if (string.IsNullOrEmpty(bootstrapToken))
        {
            logger.LogError("Unable to get bootstrap token - login failed.");
            ArgumentException.ThrowIfNullOrEmpty("bootstrapToken");
        }

        try
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bootstrapToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            query = HttpUtility.HtmlEncode(query);
            logger.LogInformation("Calling sharepoint search with query {Query}", query);
            var result = await httpClient.GetStringAsync($"_api/search/query?queryText={query}");
            logger.LogInformation("Returned from sharepoint call with {Result}", result);
            if (string.IsNullOrEmpty(result)) return [];
            var results = JsonConvert.DeserializeObject<List<SearchModel>>(result);
            logger.LogInformation("Deserialized results {ResultCount}", result.Length);
            return results ?? [];
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        return new List<SearchModel>();
    }
}