using Feedback.DomainServices.CreateOwnerToken.Contracts;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using TelegramBotClient = Telegram.Bot.TelegramBotClient;

namespace Feedback.ExternalServices.Telegram;

public static class ServiceCollectionExtensions
{
    private static IMediator Mediator;

    private static readonly ReplyKeyboardMarkup KeyboardMarkup = new ReplyKeyboardMarkup(
        new List<KeyboardButton[]>()
        {
            new KeyboardButton[]
            {
                new KeyboardButton("/help"),
                new KeyboardButton("/start")
            },
            new KeyboardButton[]
            {
                new KeyboardButton("/getHtml"),
                new KeyboardButton("/getToken")
            }
        })
    {
        // автоматическое изменение размера клавиатуры, если не стоит true,
        // тогда клавиатура растягивается чуть ли не до луны,
        // проверить можете сами
        ResizeKeyboard = true,
    };

    public static IServiceCollection AddTelegramClient(this IServiceCollection services, IMediator mediator, IConfiguration configuration)
    {
        Mediator = mediator;

        var botClient = new TelegramBotClient("7782262843:AAEJtz6ANnebLtgvHlpBGLMfaAHZGDJRkqk");
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery
            },
            DropPendingUpdates = true
        };

        using var cts = new CancellationTokenSource();

        botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);

        services.AddSingleton<ITelegramBotClient>(botClient);

        services.AddSingleton<ITelegramClient, TelegramClient>();

        return services;
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    // эта переменная будет содержать в себе все связанное с сообщениями
                    var message = update.Message;

                    // From - это от кого пришло сообщение
                    var user = message.From;

                    // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                    // Chat - содержит всю информацию о чате
                    var chat = message.Chat;

                    // Добавляем проверку на тип Message
                    switch (message.Type)
                    {
                        // Тут понятно, текстовый тип
                        case MessageType.Text:
                        {
                            // тут обрабатываем команду /start
                            if (message.Text == "/start")
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Выберите одно из следующих действий:\n" +
                                    "/start для повторного запуска\n" +
                                    "/help для отображения инструкции по интеграции\n" +
                                    "/getHtml для получения встаиваемого html-кода модуля\n" +
                                    "/getToken для получения личного токена",
                                    replyMarkup: KeyboardMarkup); // опять передаем клавиатуру в параметр replyMarkup

                                return;
                            }

                            // тут обрабатываем команду /help
                            if (message.Text == "/help")
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Для интеграции с модулем необходимо:\n" +
                                    "1. Получить html-код от бота\n" +
                                    "2. Получить личный токен от бота\n" +
                                    "3. Подставить токен в html-код\n" +
                                    "4. Встроить html-код в нужное место на странице вашего сайта",
                                    replyMarkup: KeyboardMarkup); // опять передаем клавиатуру в параметр replyMarkup

                                return;
                            }

                            // тут обрабатываем команду /getHtml
                            if (message.Text == "/getHtml")
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    @"<button id=""first-feedback"" data-feedback-trigger data-feedback-meta='{""user"":{""id"":""your-token""}}'>Оставить отзыв</button>
<script src=""./feedback.js""></script>",
                                    replyMarkup: KeyboardMarkup); // опять передаем клавиатуру в параметр replyMarkup

                                return;
                            }

                            // тут обрабатываем команду /getToken
                            if (message.Text == "/getToken")
                            {
                                var getTokenRequest = new CreateOwnerTokenRequest(chat.Id);

                                CreateOwnerTokenResponse response = await Mediator.Send(getTokenRequest, cancellationToken: cancellationToken);

                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    response.ClientToken.ToString(),
                                    replyMarkup: KeyboardMarkup); // опять передаем клавиатуру в параметр replyMarkup

                                return;
                            }

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Для начала введите /start\n");

                            return;
                        }

                        // Добавил default , чтобы показать вам разницу типов Message
                        default:
                        {
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Используй только текст!");

                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        string errorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}