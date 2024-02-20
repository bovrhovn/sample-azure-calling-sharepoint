using System.Net.Http.Headers;
using System.Web;
using Azure.Core;
using Azure.Identity;
using SearchSharepoint.CLI;
using Spectre.Console;

AnsiConsole.WriteLine("Logging into the application to go through CLI...");
// get data from environment variables
var authOptions = GetData();

if (authOptions == null)
{
    AnsiConsole.Write("[red]Please set the environment variables and try again[/]");
    return;
}

// CLI authentication
var cliCredentialOptions = new AzureCliCredentialOptions
{
    TenantId = authOptions.TenantId
};
var auth = new AzureCliCredential(cliCredentialOptions);

# region Other Options

// var auth = new OnBehalfOfCredential(authOptions.TenantId, authOptions.ClientId, authOptions.ClientSecret,
//     authOptions.Scope);

//username and password
// var auth = new UsernamePasswordCredential(authOptions.Username, authOptions.Password, authOptions.TenantId,
//     authOptions.ClientId, new TokenCredentialOptions());

//var tokens = await auth.GetTokenAsync(new TokenRequestContext([authOptions.Scope]));

var cleanSpUrl = authOptions.SharepointUrl.Replace("https://", "").Replace("/", "");
AnsiConsole.WriteLine(cleanSpUrl);
//var scope = $"00000003-0000-0ff1-ce00-000000000000/{cleanSpUrl}@{authOptions.TenantId}";
//var scope = "00000003-0000-0ff1-ce00-000000000000";

# endregion

var scope = $"{authOptions.SharepointUrl}.default";
var tokens = await auth.GetTokenAsync(new TokenRequestContext([scope]), CancellationToken.None);
var bootstrapToken = tokens.Token;
AnsiConsole.Write($"Token received successfully {bootstrapToken}");

//call to sharepoint search
var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bootstrapToken);
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
AnsiConsole.WriteLine();
var query = AnsiConsole.Ask<string>("Enter the search query: ", "csa")
    .Trim();
AnsiConsole.Write($"Searching with query: [green]{query}[/]");
query = HttpUtility.HtmlEncode(query);
AnsiConsole.WriteLine();
// sending the request to sharepoint search
var requestUri = $"{authOptions.SharepointUrl}_api/search/query?queryText='{query}'";
var result = await client.GetStringAsync(requestUri);
AnsiConsole.WriteLine("Data received from sharepoint search");
AnsiConsole.WriteLine(result);

AuthOptionsWithUsername? GetData()
{
    var tenantId = Environment.GetEnvironmentVariable("TENANTID");
    if (string.IsNullOrEmpty(tenantId))
    {
        AnsiConsole.Write("[red]TENANTID[/red] environment variable is not set");
        AnsiConsole.WriteLine();
        return null;
    }

    var clientId = Environment.GetEnvironmentVariable("CLIENTID");
    if (string.IsNullOrEmpty(clientId))
    {
        AnsiConsole.Write("[red]CLIENTID[/red] environment variable is not set");
        AnsiConsole.WriteLine();
        return null;
    }

    var secret = Environment.GetEnvironmentVariable("SECRET");
    if (string.IsNullOrEmpty(secret))
    {
        AnsiConsole.Write("[red]SECRET[/red] environment variable is not set");
        AnsiConsole.WriteLine();
        return null;
    }

    var sharepointUrl = Environment.GetEnvironmentVariable("SHAREPOINTURL");
    if (string.IsNullOrEmpty(sharepointUrl))
    {
        AnsiConsole.Write("[red]SHAREPOINTURL[/red] environment variable is not set");
        AnsiConsole.WriteLine();
        return null;
    }

    var username = Environment.GetEnvironmentVariable("USERNAME");
    if (string.IsNullOrEmpty(username))
    {
        AnsiConsole.Write("[red]USERNAME[/red] environment variable is not set");
        AnsiConsole.WriteLine();
        return null;
    }

    var password = Environment.GetEnvironmentVariable("PASSWORD");
    if (string.IsNullOrEmpty(password))
    {
        AnsiConsole.Write("[red]PASSWORD[/red] environment variable is not set");
        AnsiConsole.WriteLine();
        return null;
    }

    //return new AuthOptions(tenantId, clientId, secret, "https://graph.microsoft.com/.default");
    return new AuthOptionsWithUsername(tenantId, clientId, secret,
        $"https://login.microsoftonline.com/{tenantId}/.default",
        sharepointUrl, username, password);
}