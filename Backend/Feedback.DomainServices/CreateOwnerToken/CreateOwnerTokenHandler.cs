using Feedback.Domain;
using Feedback.DomainServices.CreateOwnerToken.Contracts;
using Feedback.Repository.Postgres;

using MediatR;

namespace Feedback.DomainServices.CreateOwnerToken;

internal sealed class CreateOwnerTokenHandler : IRequestHandler<CreateOwnerTokenRequest, CreateOwnerTokenResponse>
{
    private readonly IFeedbackOwnersRepository _feedbackOwnersRepository;

    public CreateOwnerTokenHandler(IFeedbackOwnersRepository feedbackOwnersRepository)
    {
        _feedbackOwnersRepository = feedbackOwnersRepository;
    }

    public async Task<CreateOwnerTokenResponse> Handle(CreateOwnerTokenRequest request, CancellationToken cancellationToken)
    {
        var clientToken = Guid.NewGuid();

        ClientInfo clientInfo = await _feedbackOwnersRepository.AddChannel(clientToken, request.ChannelId, cancellationToken);

        return new CreateOwnerTokenResponse(clientInfo.ClientId);
    }
}