using MediatR;

namespace Feedback.DomainServices.CreateOwnerToken.Contracts;

public record CreateOwnerTokenRequest(long ChannelId) : IRequest<CreateOwnerTokenResponse>;