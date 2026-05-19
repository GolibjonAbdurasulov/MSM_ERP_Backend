using Services.ViewModels.TelegramChatViewModels;

namespace Services.Interfaces;

public interface ITelegramChatService
{
    public Task<List<TelegramChatViewModel>> GetAllTelegramChats();   
}