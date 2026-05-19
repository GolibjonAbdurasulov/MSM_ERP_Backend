using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Services.Services;

// TelegramBackgroundService.cs
public class TelegramBackgroundService : BackgroundService, IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TelegramBackgroundService> _logger;

    public TelegramBackgroundService(
        ITelegramBotClient botClient,
        IServiceScopeFactory scopeFactory,
        ILogger<TelegramBackgroundService> logger)
    {
        _botClient = botClient;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Telegram bot started");

        _botClient.StartReceiving(
            updateHandler: this,        // IUpdateHandler
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                DropPendingUpdates = true
            },
            cancellationToken: stoppingToken);

        return Task.CompletedTask;
    }

    // IUpdateHandler.HandleUpdateAsync
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is null)
            return;

        _logger.LogInformation(
            "Update keldi! Type: {Type}, ChatId: {ChatId}",
            update.Type,
            update.Message.Chat.Id);

        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider
            .GetRequiredService<TelegramUpdateHandler>();

        await handler.HandleAsync(update);
    }

    // IUpdateHandler.HandleErrorAsync
    public Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,   // v22 da bu parameter qo'shildi!
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram bot xatosi. Source: {Source}", source);
        return Task.CompletedTask;
    }
}