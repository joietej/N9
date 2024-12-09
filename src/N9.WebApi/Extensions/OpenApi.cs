using Microsoft.OpenApi.Models;

namespace N9.WebApi.Extensions;

public static class OpenApi
{
    public static IServiceCollection AddOpenApiWithSecurityScheme(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, token) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: 'Authorization: Bearer {token}'"
                });

                return Task.CompletedTask;
            });
        });

        return services;
    }
}