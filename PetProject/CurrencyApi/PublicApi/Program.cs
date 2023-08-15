using Fuse8_ByteMinds.SummerSchool.PublicApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var webHost = WebHost
    .CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseKestrel((builderContext, options) =>
    {
        int grpcPort = builderContext.Configuration.GetValue<int>("GrpcPort");

        options.ConfigureEndpointDefaults(p =>
        {
            p.Protocols = p.IPEndPoint.Port == grpcPort
                ? HttpProtocols.Http2
                : HttpProtocols.Http1;
        });
    })
    .Build();

await webHost.RunAsync();