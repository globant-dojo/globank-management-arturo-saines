using dojo.net.bp.application.ioc;
using dojo.net.bp.api.Extentions;
using dojo.net.bp.infrastructure.extentions;
using dojo.net.bp.infrastructure.ioc;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;


using Microsoft.Extensions.Diagnostics.HealthChecks;
using dojo.net.bp.domain.entities.Settings;
using dojo.net.bp.application.interfaces.DbContexts;
using Microsoft.EntityFrameworkCore;
using dojo.net.bp.infrastructure.data.Context;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using dojo.net.bp.application.Formatters;
using Fonlow.DateOnlyExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                    options.SerializerSettings.Converters.Add(new dojo.net.bp.application.Formatters.DateOnlyJsonConverter());
                    options.SerializerSettings.Converters.Add(new DateOnlyNullableJsonConverter());
                }
            ); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(builder.Configuration["OpenApi:info:version"], new OpenApiInfo
    {
        Version = builder.Configuration["OpenApi:info:version"],
        Title = builder.Configuration["OpenApi:info:title"],
        Description = builder.Configuration["OpenApi:info:description"],
        TermsOfService = new Uri(builder.Configuration["OpenApi:info:termsOfService"]),
        Contact = new OpenApiContact
        {
            Name = builder.Configuration["OpenApi:info:contact:name"],
            Url = new Uri(builder.Configuration["OpenApi:info:contact:url"]),
            Email = builder.Configuration["OpenApi:info:contact:email"]
        },
        License = new OpenApiLicense
        {
            Name = builder.Configuration["OpenApi:info:License:name"],
            Url = new Uri(builder.Configuration["OpenApi:info:License:url"])
        }
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddDbContextFactory<BP_DBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("BPDB"));
});
//builder.Services.AddTransient<ABP_DBContext>(provider => provider.GetService<BP_DBContext>());


//Dependencias propias de Servicio
builder.Services.RegisterDependencies();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();



builder.Services.AddHealthChecks();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"v2");
    });
}


app.ConfigureMetricServer();
app.ConfigureExceptionHandler();
app.UseAuthorization();

app.MapHealthChecks("/health/readiness", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
});

app.MapHealthChecks("/health/liveness", new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    Predicate = _ => false
});



app.MapControllers();

//Globalization for dates
var defaultCulture = new CultureInfo("es-EC");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
});




app.Run();
