using System.Globalization;
using System.Reflection;
using Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Path = System.IO.Path;

namespace Infrastructure.Extensions.Localization;

public static class LocalizationExtensions {
    public static void AddLocalizationMessages(this IServiceCollection svc) {
        var domainLayerPath = GetPathDomainLayer();
        svc.AddLocalization(options => options.ResourcesPath = domainLayerPath);
        svc.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("es"),
            };

            options.DefaultRequestCulture = new RequestCulture("en");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        svc.AddSingleton<LocalizationMiddleware>();
    }

    private static string GetPathDomainLayer()
    {
        var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var projectDirectory = Path.GetFullPath(Path.Combine(assemblyDirectory!, @"..\..\..\"));
        var domainLayerPath = Path.Combine(projectDirectory, ApiConstants.DomainProject);
        return domainLayerPath;
    }

    public static void UseLocalizationMessages(this IApplicationBuilder app)
    {
        app.UseRequestLocalization();
        app.UseMiddleware<LocalizationMiddleware>();
    }
}