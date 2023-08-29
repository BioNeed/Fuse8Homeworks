using System.Text.Json.Serialization;
using Audit.Http;
using Audit.NET.Serilog.Providers;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts.GrpcContracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Filters;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using UserDataAccessLibrary.Constants;
using UserDataAccessLibrary.Contracts;
using UserDataAccessLibrary.Database;
using UserDataAccessLibrary.Repositories;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi;

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
        IConfigurationSection apiSettingsSection = _configuration.GetRequiredSection("ApiSettings");
        services.Configure<ApiSettingsModel>(apiSettingsSection);

        services.AddScoped<IGrpcCurrencyService, GrpcCurrencyService>();
        services.AddScoped<ISettingsRepository, SettingsRepository>();
        services.AddScoped<ISettingsService, SettingsService>();
        services.AddScoped<IFavouritesRepository, FavouritesRepository>();
        services.AddScoped<IFavouritesService, FavouritesService>();

        services.AddGrpcClient<GrpcCurrency.GrpcCurrencyClient>((provider, options) =>
        {
            var apiSettings = provider.GetRequiredService<IOptionsMonitor<ApiSettingsModel>>();
            options.Address = new Uri(apiSettings.CurrentValue.BaseAddress);
        }).AddAuditHandler(audit =>
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

        services.AddHealthChecks().AddCheck("LogHealthy", () =>
        {
            Console.WriteLine("Healthy");
            return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy();
        });

        services.AddDbContext<UserDbContext>(
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
                                LibraryConstants.SchemaNames.User);
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

        app.UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/healthcheck");
            });
    }
}