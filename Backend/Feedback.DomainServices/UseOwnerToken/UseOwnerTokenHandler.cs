using Feedback.DomainServices.UseOwnerToken.Contracts;
using Feedback.Repository.Postgres;

using MediatR;

namespace Feedback.DomainServices.UseOwnerToken;

internal sealed class UseOwnerTokenHandler : IRequestHandler<UseOwnerTokenRequest, UseOwnerTokenResponse>
{
    private readonly IFeedbackOwnersRepository _feedbackOwnersRepository;

    public UseOwnerTokenHandler(IFeedbackOwnersRepository feedbackOwnersRepository)
    {
        _feedbackOwnersRepository = feedbackOwnersRepository;
    }

    public async Task<UseOwnerTokenResponse> Handle(UseOwnerTokenRequest request, CancellationToken cancellationToken)
    {

        await _feedbackOwnersRepository.AddChannel(request.ClientToken, request.ChannelId, cancellationToken);

        return new UseOwnerTokenResponse(request.ClientToken);
    }
}