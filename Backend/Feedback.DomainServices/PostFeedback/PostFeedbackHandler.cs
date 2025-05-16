using Feedback.Domain;
using Feedback.DomainServices.PostFeedback.Contracts;
using Feedback.ExternalServices;
using Feedback.Repository.Clickhouse;
using Feedback.Repository.Postgres;

using MediatR;

namespace Feedback.DomainServices.PostFeedback;

internal sealed class PostFeedbackHandler : IRequestHandler<PostFeedbackInternalRequest>
{
    private readonly ITelegramClient _telegramClient;
    private readonly IToneModelClient _toneModelClient;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IFeedbackOwnersRepository _feedbackOwnersRepository;

    public PostFeedbackHandler(ITelegramClient telegramClient, IToneModelClient toneModelClient, IFeedbackRepository feedbackRepository, IFeedbackOwnersRepository feedbackOwnersRepository)
    {
        _telegramClient = telegramClient;
        _toneModelClient = toneModelClient;
        _feedbackRepository = feedbackRepository;
        _feedbackOwnersRepository = feedbackOwnersRepository;
    }

    public async Task Handle(PostFeedbackInternalRequest request, CancellationToken cancellationToken)
    {
        Tone tone = await _toneModelClient.GetFeedbackTone(request.Message, cancellationToken);

        string feedbackMessage = "Получен новый отзыв!\n" +
            $"Тональность: {tone}\n" +
            $"Текст отзыва: {request.Message}";

        long[] channelIds = await _feedbackOwnersRepository.GetChannels(request.ClientToken, cancellationToken);

        foreach (long channelId in channelIds)
        {
            await _telegramClient.SendMessage(channelId, feedbackMessage, cancellationToken);
        }

        await _feedbackRepository.SaveFeedbackMessage(request.ClientToken, request.Message, tone, cancellationToken);
    }
}