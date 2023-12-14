using System.Text.Json.Serialization;

namespace SearchSharepoint.Web.Models;

public class AzureAdToken
{
    [JsonPropertyName("resource")]
    public string Resource { get; set; }
    public string access_token { get; set; }
}