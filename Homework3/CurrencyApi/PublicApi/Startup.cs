using System.Text.Json.Serialization;
using Audit.Http;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Constants;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Filters;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Services.FindingExceptions;
using Microsoft.OpenApi.Models;

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
        IConfigurationSection section = _configuration.GetRequiredSection("Settings");
        services.Configure<CurrencyConfigurationModel>(section);

        string apiKey = _configuration[ApiConstants.ApiKeys.Default];
        string baseAddress = _configuration[ApiConstants.Uris.BaseAddress];

        services.AddHttpClient(ApiConstants.HttpClientsNames.CurrencyApi,
            client =>
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Add("apikey", apiKey);
            })
            .AddAuditHandler(audit =>
                audit.IncludeRequestHeaders()
                .IncludeResponseHeaders()
                .IncludeRequestBody()
                .IncludeResponseBody()
                .IncludeContentHeaders());

        services.AddSingleton<IRequestSender, HttpClientRequestSender>();
        services.AddSingleton<IResponseHandler, BadResponseHandler>();
        services.AddSingleton<ICheckingBeforeRequests, CheckingRequestsAvailability>();
        services.AddTransient<DateValidator>();
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