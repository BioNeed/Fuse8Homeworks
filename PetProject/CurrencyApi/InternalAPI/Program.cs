using InternalAPI;
using InternalAPI.Constants;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        webBuilder =>
        {
            webBuilder.UseStartup<Startup>()
                .UseKestrel((builderContext, options) =>
                {
                    int grpcPort = builderContext.Configuration.GetValue<int>(
                        ApiConstants.PortNames.GrpcPort);

                    options.ConfigureEndpointDefaults(p =>
                    {
                        p.Protocols = p.IPEndPoint.Port == grpcPort
                            ? HttpProtocols.Http2
                            : HttpProtocols.Http1;
                    });
                });
        })
    .Build();

await host.RunAsync();