using Telegram.Bot;
using Telegram.Bot.Types;

namespace Feedback.ExternalServices.Telegram;

public class TelegramClient : ITelegramClient
{
    private readonly ITelegramBotClient _botClient;

    public TelegramClient(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMessage(long chatId, string message, CancellationToken cancellationToken)
    {
        var chat = new ChatId(chatId);

        await _botClient.SendMessage(
            chat,
            message,
            cancellationToken: cancellationToken);
    }
}