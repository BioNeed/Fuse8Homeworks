using Fuse8_ByteMinds.SummerSchool.PublicApi.Middlewares;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

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
		services.AddControllers()

			// Добавляем глобальные настройки для преобразования Json
			.AddJsonOptions(
				options =>
				{
					// Добавляем конвертер для енама
					// По умолчанию енам преобразуется в цифровое значение
					// Этим конвертером задаем перевод в строковое занчение
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});
		;
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(
			c =>
			{
				c.SwaggerDoc(
					"v1",
					new OpenApiInfo()
					{
						Title = "Апи",
						Version = "v1",
						Description = "API by Alexey Goncharov"
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

        app.UseMiddleware<LoggingMiddleware>();

        app.UseRouting()
			.UseEndpoints(endpoints => endpoints.MapControllers());
	}
}