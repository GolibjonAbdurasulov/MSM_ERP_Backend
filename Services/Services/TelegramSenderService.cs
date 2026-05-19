using Core.Attributes;
using Telegram.Bot;

namespace Services.Services;

[Injectable]
public class TelegramSenderService : ITelegramSender
{
    private ITelegramBotClient _telegramBotClient;

    public TelegramSenderService(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task SendJobTextMessageAsync(long chatId, string message)
    {
        await _telegramBotClient.SendMessage(
            chatId: chatId,
            text: message
        );
    }
}