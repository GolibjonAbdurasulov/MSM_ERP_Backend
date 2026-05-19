
public interface ITelegramSender
{
    Task SendJobTextMessageAsync(long chatId, string message);
}
