using System.Reflection;
using DigitalWater.Api.Configurations.Mongo;
using DigitalWater.Api.Middlewares;
using DigitalWater.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace DigitalWater.Api.Extensions.Application;

/// <summary>
/// ApplicationBuilder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Использование базовых сервисов 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="provider"></param>
    /// <param name="env"></param>
    public static void UseBaseServices(this IApplicationBuilder app, IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        // app.UseJwtAuthentication();

        #region Middleware

        app.UseMiddleware<RequestLoggingMiddleware>();

        #endregion

        app.UseSwaggerService(provider);

        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller:slugify}/{action:slugify}/{id?}");
        });
    }

    /// <summary>
    /// Внедрнение автогенерируемой документации API - Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <param name="provider"></param>
    private static void UseSwaggerService(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        var isDevelopment = app.ApplicationServices.GetService<IWebHostEnvironment>()!.IsDevelopment();

        var assemblyName = Assembly.GetEntryAssembly()!.GetName().GetNameDashCase();
        
        // var prefixApi = isDevelopment ? "" : $"/api/{assemblyName}";
        const string prefixApi = ""; // вместо верхней строчки, пока хардкод (для нескольких проектов нужно будет менять)

        app.UseSwagger();

        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"{prefixApi}/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                options.DocExpansion(DocExpansion.None);
            });
    }

    /// <summary>
    /// Использование JWT токенов
    /// </summary>
    /// <param name="app"></param>
    private static void UseJwtAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}