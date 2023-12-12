using System.ComponentModel.DataAnnotations;

namespace SearchSharepoint.Web.Options;

public class AzureAdOptions
{
    public string Instance { get; set; } = "https://login.microsoftonline.com/";
    [Required(ErrorMessage = "Domain is required")]
    public string Domain { get; set; }
    [Required(ErrorMessage = "TenantId is required")]
    public string TenantId { get; set; }
    [Required(ErrorMessage = "ClientId is required")]
    public string ClientId { get; set; }
    [Required(ErrorMessage = "CallbackPath is required")]
    public string CallbackPath { get; set; }
    public string SignedOutCallbackPath { get; set; }
    [Required(ErrorMessage = "ClientSecret is required")]
    public string Secret { get; set; }
    [Required(ErrorMessage = "Azure SubscriptionId is required")]
    public string SubscriptionId { get; set; }
    [Required(ErrorMessage = "URL to SharePoint is required")]
    public string ApplicationIdUri { get; set; }
}