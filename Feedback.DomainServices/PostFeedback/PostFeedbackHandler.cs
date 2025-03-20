using Feedback.DomainServices.PostFeedback.Contracts;
using Feedback.ExternalServices;

using MediatR;

namespace Feedback.DomainServices.PostFeedback;

internal sealed class PostFeedbackHandler : IRequestHandler<PostFeedbackInternalRequest>
{
    private readonly ITelegramClient _telegramClient;

    public PostFeedbackHandler(ITelegramClient telegramClient)
    {
        _telegramClient = telegramClient;
    }

    public async Task Handle(PostFeedbackInternalRequest request, CancellationToken cancellationToken)
    {
        await _telegramClient.SendMessage(request.ChatId, request.Message, cancellationToken);
    }
}