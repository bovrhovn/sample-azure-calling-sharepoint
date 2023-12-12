using SearchSharepoint.Web.Models;

namespace SearchSharepoint.Web.Services;

public interface ISharepointSearchServices
{
    Task<string> LoginAsync();
    Task<List<SearchModel>> SearchAsync(string query);
}