using Asp.Versioning;
using Gateway.Swagger.OpenApi;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gateway.Swagger;

public static class Swagger
{
    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        builder.Services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            })
            .AddApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });
    }

    public static void UseSwagger(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return;
        }
        
        SwaggerBuilderExtensions.UseSwagger(app);
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
    }
}