namespace Services.ViewModels.TelegramChatViewModels;

public class TelegramChatCreationViewModel
{
    public long ChatId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string ChatType { get; set; } = "group";
    public bool BotAdded { get; set; }
    public bool CanSendMessage { get; set; }
    public bool IsActive { get; set; } = true;
}