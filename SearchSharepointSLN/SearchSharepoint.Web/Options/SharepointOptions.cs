using System.ComponentModel.DataAnnotations;

namespace SearchSharepoint.Web.Options;

public class SharepointOptions
{
    [Required(ErrorMessage = "The SharePoint URL is required.")]
    public string TenantUrl { get; set; }
}