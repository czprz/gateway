﻿using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gateway.Swagger.OpenApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
    
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text = new StringBuilder("APIs for authentication and registering of routes.");
        var info = new OpenApiInfo
        {
            Title = "Gateway API",
            Version = description.ApiVersion.ToString(),
            License = new OpenApiLicense { Name = "The Unlicense", Url = new Uri("https://unlicense.org/") },
            Contact = new OpenApiContact
            {
                Name = "Casper Overholm Elkrog",
                Email = "support.open-source+gateway@elkrog.me"
            }
        };

        if (description.IsDeprecated)
        {
            text.Append(" This API version has been deprecated.");
        }

        info.Description = text.ToString();

        return info;
    }
}