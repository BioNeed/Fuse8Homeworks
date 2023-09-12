using Fuse8_ByteMinds.SummerSchool.PublicApi;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .Build();

await host.RunAsync();