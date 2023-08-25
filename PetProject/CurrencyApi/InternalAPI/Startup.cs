using System.Text.Json.Serialization;
using Audit.Http;
using Audit.NET.Serilog.Providers;
using CurrenciesDataAccessLibrary.Contracts;
using CurrenciesDataAccessLibrary.DataAccess;
using CurrenciesDataAccessLibrary.Repositories;
using InternalAPI.Constants;
using InternalAPI.Contracts;
using InternalAPI.Filters;
using InternalAPI.Middlewares;
using InternalAPI.Models;
using InternalAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

namespace InternalAPI;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        IConfigurationSection settingsSection = _configuration.GetRequiredSection("Settings");
        services.Configure<ApiInfoModel>(settingsSection);

        IConfigurationSection apiSettingsSection = _configuration.GetRequiredSection("ApiSettings");
        services.Configure<ApiSettingsModel>(apiSettingsSection);

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

        services.AddHttpClient(ApiConstants.HttpClientNames.CurrencyApi, (provider, client) =>
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

        services.AddScoped<ICurrencyAPI, CurrencyService>();
        services.AddScoped<IGettingApiConfigService, CurrencyService>();
        services.AddScoped<ICachedCurrencyAPI, CachedCurrencyService>();
        services.AddScoped<IHealthCheck, HealthCheckService>();
        services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();

        services.AddGrpc();
        services.AddHttpContextAccessor();

        services.AddDbContext<CurrenciesDbContext>(
            optionsBuilder =>
            {
                if (_environment.IsDevelopment())
                {
                    optionsBuilder.EnableDetailedErrors();
                    optionsBuilder.EnableSensitiveDataLogging();
                }

                optionsBuilder.UseNpgsql(
                    connectionString: _configuration.GetConnectionString(
                        ApiConstants.ConnectionStringNames.DockerSummerSchool),
                    npgsqlOptionsAction: sqlOptionsBuilder =>
                    {
                        sqlOptionsBuilder.EnableRetryOnFailure()
                            .MigrationsHistoryTable(
                                HistoryRepository.DefaultTableName,
                                ApiConstants.SchemaNames.Currencies);
                    })
                .UseSnakeCaseNamingConvention()
                .UseAllCheckConstraints()
                .ConfigureWarnings(
                    config => config
                        .Log(
                            (CoreEventId.StartedTracking, LogLevel.Information),
                            (RelationalEventId.TransactionRolledBack, LogLevel.Warning),
                            (RelationalEventId.CommandCanceled, LogLevel.Warning)));
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

        app.UseWhen(
            predicate: context => context.Connection.LocalPort ==
                _configuration.GetValue<int>(ApiConstants.PortNames.GrpcPort),
            configuration: grpcBuilder =>
            {
                grpcBuilder.UseRouting().UseEndpoints(endpoints =>
                    endpoints.MapGrpcService<GrpcCurrencyService>());
            });

        app.UseWhen(
            predicate: context => context.Connection.LocalPort ==
                _configuration.GetValue<int>(ApiConstants.PortNames.RestApiPort),
            configuration: restApiBuilder =>
            {
                restApiBuilder.UseRouting().UseEndpoints(endpoints =>
                    endpoints.MapControllers());
            });
    }
}