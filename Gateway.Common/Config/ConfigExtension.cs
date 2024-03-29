using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Gateway.Common.Config;

public static class ConfigExtension
{
    public static void AddConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<GatewayConfig>()
            .Bind(builder.Configuration.GetSection("Gateway"))
            .ValidateDataAnnotations();
        
        builder.Services.AddSingleton<IConfig, Config>();
    }
}