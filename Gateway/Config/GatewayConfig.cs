using System.ComponentModel.DataAnnotations;

namespace Gateway.Config;

public record GatewayConfig
{
    [Required(ErrorMessage = "The authority is required.")]
    [Url(ErrorMessage = "The Authority must be a valid URL.")]
    [RegularExpression(@".+/$", ErrorMessage = "The URL must end with a trailing slash.")]
    public string Authority { get; init; } = "";
    
    [Required(ErrorMessage = "The authority discovery URL is required.")]
    [RegularExpression(@".*\.well-known/openid-configuration$", ErrorMessage = "The path must end with '.well-known/openid-configuration'.")]
    public string AuthorityDiscoveryUrl { get; init; } = "";

    [Range(0, 60, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
    public int? SessionTimeoutInMin { get; init; }
    
    [EnumDataType(typeof(TokenExchangeStrategy), ErrorMessage = "The token exchange strategy must be either 'None' or 'TokenExchange'.")]
    public TokenExchangeStrategy? TokenExchangeStrategy { get; init; }

    [Required(ErrorMessage = "The client ID is required.")]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "The client ID name must contain only alphanumeric characters, dots, underscores, and dashes.")]
    public string ClientId { get; init; }
    
    [RegularExpression(@"^[a-zA-Z0-9\-._~+/]{43,128}$", ErrorMessage = "The client secret must be a base64-encoded string between 43 and 128 characters long.")]
    public string? ClientSecret { get; init; }
    
    [RegularExpression(@"^[a-zA-Z0-9._/-]+$", ErrorMessage = "The scope must contain only alphanumeric characters, dots, underscores, slashes, and dashes.")]
    public string? Scopes { get; init; }
}