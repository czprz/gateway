using System.ComponentModel.DataAnnotations;

namespace Gateway.Config.Validators;

public class OpenIdConfigurationPathAttribute : ValidationAttribute
{
    private const string ExpectedPath = ".well-known/openid-configuration";

    public string InvalidPathErrorMessage { get; set; } = "The {0} field must be a valid path.";
    public string MissingPathErrorMessage { get; set; } = "The {0} field must include \".well-known/openid-configuration\".";

    public override bool IsValid(object value)
    {
        if (value is not string path)
        {
            return false;
        }

        if (!path.Contains(ExpectedPath, StringComparison.OrdinalIgnoreCase))
        {
            ErrorMessage = string.Format(MissingPathErrorMessage, "AuthorityDiscoveryUrl");
            return false;
        }

        if (path.StartsWith('/') || path.EndsWith('/'))
        {
            ErrorMessage = string.Format(InvalidPathErrorMessage, "AuthorityDiscoveryUrl");
            return false;
        }

        return true;
    }
}