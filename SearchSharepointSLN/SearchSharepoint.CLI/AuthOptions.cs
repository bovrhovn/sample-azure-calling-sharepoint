namespace SearchSharepoint.CLI;

record AuthOptions(string TenantId, string ClientId, string ClientSecret, string Scope, string SharepointUrl = "");

record AuthOptionsWithUsername(
    string TenantId,
    string ClientId,
    string ClientSecret,
    string Scope,
    string SharepointUrl,
    string Username,
    string Password) : AuthOptions(TenantId, ClientId, ClientSecret, Scope, SharepointUrl);