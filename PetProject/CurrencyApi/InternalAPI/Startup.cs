using System.Text.Json.Serialization;
using Audit.Http;
using Audit.NET.Serilog.Providers;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Filters;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        IConfigurationSection settingsSection = _configuration.GetRequiredSection("Settings");
        services.Configure<CurrencyConfigurationModel>(settingsSection);

        IConfigurationSection apiSettingsSection = _configuration.GetRequiredSection("ApiSettings");
        services.Configure<ApiSettingsModel>(apiSettingsSection);

        services.AddHttpClient<ICurrencyService, CurrencyService>((provider, client) =>
            {
                var apiSettings = provider.GetRequiredService<IOptionsMonitor<ApiSettingsModel>>();
                client.BaseAddress = new Uri(apiSettings.CurrentValue.BaseAddress);
                client.DefaultRequestHeaders.Add("apikey", apiSettings.CurrentValue.ApiKey);
            })
            .AddAuditHandler(audit =>
                audit
                .AuditDataProvider(new SerilogDataProvider(config =>
                    config.Logger(new LoggerConfiguration()
                        .ReadFrom.Configuration(_configuration)
                        .CreateLogger())))
                .IncludeRequestHeaders()
                .IncludeResponseHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody()
                .IncludeContentHeaders());

        services.AddControllers(options =>
            options.Filters.Add(typeof(GlobalExceptionFilter)))

            // Добавляем глобальные настройки для преобразования Json
            .AddJsonOptions(
                options =>
                {
                    // Добавляем конвертер для енама
                    // По умолчанию енам преобразуется в цифровое значение
                    // Этим конвертером задаем перевод в строковое занчение
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo()
                    {
                        Title = "[Fuse8_ByteMinds Internship] API for working with Currencyapi API",
                        Version = "v1",
                        Description = "Developed by Alexey Goncharov",
                    });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<LoggingRequestsMiddleware>();

        app.UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}