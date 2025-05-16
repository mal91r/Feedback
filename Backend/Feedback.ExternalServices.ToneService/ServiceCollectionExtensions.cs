using Grpc.Net.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Feedback.ExternalServices.ToneService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddToneService(this IServiceCollection services, IConfiguration configuration)
    {
        GrpcChannel channel = GrpcChannel.ForAddress("http://5.129.204.202:5002");
        var client = new Tone.ToneService.ToneServiceClient(channel);

        services.AddSingleton<Tone.ToneService.ToneServiceClient>(client);

        services.AddSingleton<IToneModelClient, ToneModelClient>();

        return services;
    }
}