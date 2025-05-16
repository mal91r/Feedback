using MediatR;

namespace Feedback.DomainServices.PostFeedback.Contracts;

public record PostFeedbackInternalRequest(Guid ClientToken, string Message) : IRequest;