using SearchSharepoint.Web.Models;

namespace SearchSharepoint.Web.Services;

public interface ISharepointSearchServices
{
    Task<string> LoginAsync();
    Task<string> LoginWithCodeAsync(string? code, string? redirectUri="https://localhost:5556/");
    Task<List<SearchModel>> SearchAsync(string query);
    Task<List<SearchModel>> SearchWithAsync(string code, string query, string redirectUrl);
}