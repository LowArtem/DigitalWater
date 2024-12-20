using DigitalWater.Api.Extensions.Application;
using DigitalWater.Api.Mappers;
using AutoMapper;
using DigitalWater.Api.Configurations.Mongo;
using DigitalWater.Core.Model;
using DigitalWater.Data.Services;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;
using Serilog;

namespace DigitalWater.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBaseModuleDi("DefaultConnection", Configuration);


        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
        
        services.Configure<List<Sensor>>(Configuration.GetSection("Sensors"));
        services.AddSingleton<DataSeeder>();

        services.AddTransient<ServiceReceivingService, ServiceReceivingService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
        IApiVersionDescriptionProvider provider,
        IWebHostEnvironment env,
        ILogger<Startup> logger,
        IHost host)
    {
        // app.MigrateDatabase(logger);
        
        var dataSeeder = app.ApplicationServices.GetRequiredService<DataSeeder>();
        dataSeeder.SeedDatabase(host);

        app.UseBaseServices(env, provider);

        app.UseSerilogRequestLogging();
        
        app.UseHttpMetrics();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
        });
    }
}