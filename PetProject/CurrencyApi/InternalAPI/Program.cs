using InternalAPI;
using Microsoft.AspNetCore;

var webHost = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .Build();

await webHost.RunAsync();