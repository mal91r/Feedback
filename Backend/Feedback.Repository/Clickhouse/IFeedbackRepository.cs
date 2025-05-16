using Feedback.Domain;

namespace Feedback.Repository.Clickhouse;

public interface IFeedbackRepository
{
    Task SaveFeedbackMessage(Guid clientId, string message, Tone tone, CancellationToken cancellationToken);
}