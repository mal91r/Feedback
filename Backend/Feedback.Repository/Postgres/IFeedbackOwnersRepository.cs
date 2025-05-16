using Feedback.Domain;

namespace Feedback.Repository.Postgres;

public interface IFeedbackOwnersRepository
{
    Task<ClientInfo> AddChannel(Guid clientId, long channelId, CancellationToken cancellationToken);
    Task<long[]> GetChannels(Guid clientId, CancellationToken cancellationToken);
}