using Feedback.Infrastructure.Repositories.Clickhouse;
using Feedback.Infrastructure.Repositories.Postgres;
using Feedback.Repository.Clickhouse;
using Feedback.Repository.Postgres;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Feedback.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IFeedbackRepository, FeedbackRepository>();
        services.AddSingleton<IFeedbackOwnersRepository, FeedbackOwnersRepository>();

        return services;
    }
}