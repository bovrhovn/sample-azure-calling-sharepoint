using System.Net;
using System.Net.Http.Headers;
using System.Security;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using SearchSharepoint.Web.Models;
using SearchSharepoint.Web.Options;

namespace SearchSharepoint.Web.Services;

public class SharepointSearchService(
    HttpClient client,
    IOptions<AzureAdOptions> azureAdOptions,
    IOptions<SharepointOptions> sharepointOptionsValue,
    ILogger<SharepointSearchService> logger)
    : ISharepointSearchServices
{
    private readonly SharepointOptions sharepointOptions = sharepointOptionsValue.Value;
    private readonly AzureAdOptions azureAdOptions = azureAdOptions.Value;

    // public async Task<string> LoginAsync()
    // {
    //     var app = ConfidentialClientApplicationBuilder.Create(azureAdOptions.ClientId)
    //         .WithClientSecret(azureAdOptions.Secret)
    //         .WithTenantId(azureAdOptions.TenantId)
    //         .WithAuthority(new Uri($"https://login.microsoftonline.com/{azureAdOptions.TenantId}"))
    //         .WithLegacyCacheCompatibility(false)
    //         .Build();
    //     var scopes = new[] { $"{sharepointOptions.TenantUrl}.default" };
    //     var token = await app.AcquireTokenForClient(scopes).ExecuteAsync();
    //     return token == null ? string.Empty : token.AccessToken;
    // }


    public async Task<string> LoginAsync()
    {
        var uri = $"https://accounts.accesscontrol.windows.net/{azureAdOptions.TenantId}/tokens/OAuth/2";
        logger.LogInformation("Logging to Azure Application with uri {Uri}", uri);
        var cleanSpUrl = sharepointOptions.TenantUrl.Replace("https://", "")
            .Replace("/", "");

        logger.LogInformation("Sharepoint url {Url}", cleanSpUrl);
        var data = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("resource",
                $"00000003-0000-0ff1-ce00-000000000000/{cleanSpUrl}@{azureAdOptions.TenantId}"),
            new("client_id", $"{azureAdOptions.ClientId}@{azureAdOptions.TenantId}"),
            new("client_secret", azureAdOptions.Secret)
        };
        var content = new FormUrlEncodedContent(data);
        try
        {
            var result = await client.PostAsync(uri, content);
            logger.LogInformation("Call to Azure AD was successful.");
            var bootStrapToken = await result.Content.ReadAsStringAsync();
            logger.LogInformation("Bootstrap token received. {BoostrapToken}", bootStrapToken);
            
            var azureAdToken = JsonConvert.DeserializeObject<AzureAdToken>(bootStrapToken);

            if (azureAdToken == null)
                throw new SecurityException("Unable to get bootstrap token - login failed.");
            
            return azureAdToken.access_token;
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            return string.Empty;
        }
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

        var list = new List<SearchModel>();

        try
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bootstrapToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            query = HttpUtility.HtmlEncode(query);
            logger.LogInformation("Calling sharepoint search with query {Query}", query);
            var requestUri = $"{sharepointOptions.TenantUrl}_api/search/query?queryText='{query}'";
            var result = await client.GetStringAsync(requestUri);
            logger.LogInformation("Returned from sharepoint call with {Result}", result);

            if (string.IsNullOrEmpty(result)) return [];

            var results = JsonConvert.DeserializeObject<SharepointResultModels>(result);
            logger.LogInformation("Deserialized results {ResultCount}", result.Length);

            results?.PrimaryQueryResult.RelevantResults.Table.Rows.ToList().ForEach(row =>
            {
                var sm = new SearchModel();

                row.Cells.ToList().ForEach(cell =>
                {
                    logger.LogInformation("Key: {Key} Value: {Value}", cell.Key, cell.Value);
                    switch (cell.Key)
                    {
                        case "Title":
                            sm.Title = cell.Value;
                            break;
                        case "HitHighlightedSummary":
                        {
                            var formattedWithoutHtml = Regex.Replace(cell.Value, "<.*?>", string.Empty);
                            sm.ShortDescription = formattedWithoutHtml;
                            break;
                        }
                        case "Path":
                            sm.MoreLink = cell.Value;
                            break;
                    }
                });
                list.Add(sm);
            });
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }

        return list;
    }
}