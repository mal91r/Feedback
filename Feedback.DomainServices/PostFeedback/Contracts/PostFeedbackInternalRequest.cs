using MediatR;

namespace Feedback.DomainServices.PostFeedback.Contracts;

public record PostFeedbackInternalRequest(long ChatId, string Message) : IRequest;