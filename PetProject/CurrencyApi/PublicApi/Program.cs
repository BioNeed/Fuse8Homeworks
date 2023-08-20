using Fuse8_ByteMinds.SummerSchool.PublicApi;
using Microsoft.AspNetCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .Build();

await host.RunAsync();