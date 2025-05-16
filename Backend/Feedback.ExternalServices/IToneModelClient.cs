using Feedback.Domain;

namespace Feedback.ExternalServices;

public interface IToneModelClient
{
    Task<Tone> GetFeedbackTone(string text, CancellationToken cancellationToken);
}