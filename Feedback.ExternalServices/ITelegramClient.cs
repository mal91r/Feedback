namespace Feedback.ExternalServices;

public interface ITelegramClient
{
    Task SendMessage(long chatId, string message, CancellationToken cancellationToken);
}