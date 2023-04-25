using Swashbuckle.AspNetCore.SwaggerGen;

namespace FluentTranslate.WebApi.Swagger
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            const string securityDefinitionName = "JwtBearer";
            var scheme = new OpenApiSecurityScheme
            {
                Description = "Please provide a valid token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            };
            var requirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = securityDefinitionName,
                        }
                    },
                    Array.Empty<string>()
                }
            };
            options.AddSecurityDefinition(securityDefinitionName, scheme);
            options.AddSecurityRequirement(requirement);
        }
    }
}
