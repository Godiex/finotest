using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Path = System.IO.Path;

namespace Infrastructure.Extensions.OpenApi;

public static class OpenApiDocumentationExtensions {
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection svc, IWebHostEnvironment env) {
        return svc.AddSwaggerGen(o =>
        {
            var openApiDocPath = Path.Combine(env.ContentRootPath, "Docs", "openapi.yaml");

            // Carga manualmente el documento YAML
            var yamlDoc = File.ReadAllText(openApiDocPath);

            // Configuración para incluir el documento YAML
            o.CustomSchemaIds(type => type.FullName); // Personalización opcional del ID del esquema
            o.DescribeAllParametersInCamelCase(); // Otras configuraciones opcionales

            // Agregar manualmente la operación con el documento YAML
            o.OperationFilter<YamlOperationFilter>(yamlDoc);
        });
    }

    public static IApplicationBuilder UseOpenApiDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Block Api"));
        return app;
    }
}

public class YamlOperationFilter : IOperationFilter
{
    private readonly string _yamlDoc;

    public YamlOperationFilter(string yamlDoc)
    {
        _yamlDoc = yamlDoc;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Agregar manualmente la extensión del documento YAML
        operation.Extensions.Add("x-wagmp-code-gen-yaml", new OpenApiString(_yamlDoc));
    }
}