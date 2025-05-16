using MediatR;

namespace Feedback.DomainServices.UseOwnerToken.Contracts;

public record UseOwnerTokenRequest(long ChannelId, Guid ClientToken) : IRequest<UseOwnerTokenResponse>;