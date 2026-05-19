using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Repositories.TelegramChatRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Telegram.Bot.Types;

namespace Services.Services;
// TelegramUpdateHandler.cs
public class TelegramUpdateHandler
{
    private readonly ITelegramChatRepository _telegramChatRepository;
    private readonly ILogger<TelegramUpdateHandler> _logger;

    public TelegramUpdateHandler(
        ITelegramChatRepository telegramChatRepository,
        ILogger<TelegramUpdateHandler> logger)
    {
        _telegramChatRepository = telegramChatRepository;
        _logger = logger;
    }

    public async Task HandleAsync(Update update)
    {
        // Message va Chat null emasligini tekshirish
        var message = update.Message;
        if (message?.Chat is null)
            return;

        var chat = message.Chat;
        _logger.LogInformation("Message received from ChatId: {ChatId}", chat.Id);

        var exists = await _telegramChatRepository
            .AnyAsync(x => x.ChatId == chat.Id);

        if (!exists)
        {
            await _telegramChatRepository.AddAsync(new TelegramChat
            {
                ChatId = chat.Id,
                Title = chat.Title,
                ChatType = chat.Type.ToString(),
                BotAdded = true,
                CanSendMessage = true
            });

            // Agar repository SaveChanges chaqirmasa, bu yerda chaqirish kerak:
            // await _telegramChatRepository.SaveChangesAsync();

            _logger.LogInformation("New chat saved: {ChatId}", chat.Id);
        }
    }
}