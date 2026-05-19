using Core.Attributes;
using DataAccess.Entities;
using DataAccess.Repositories.TelegramChatRepositories;
using Services.Interfaces;
using Services.ViewModels.TelegramChatViewModels;

namespace Services.Services;

[Injectable]
public class TelegramChatService : ITelegramChatService
{
    private readonly ITelegramChatRepository _repository;

    public TelegramChatService(ITelegramChatRepository repository)
    {
        _repository = repository;
    }

    public Task<List<TelegramChatViewModel>> GetAllTelegramChats()
    {
        var result =  _repository.GetAllAsQueryable().ToList();
        var resChats= new  List<TelegramChatViewModel>();
        foreach (TelegramChat chat in result)
        {
            resChats.Add(new TelegramChatViewModel
            {
                ChatId = chat.Id,
                Title = chat.Title,
                Username = chat.Username,
                ChatType = chat.ChatType,
                BotAdded = chat.BotAdded,
                CanSendMessage = chat.CanSendMessage,
                IsActive = chat.IsActive,
            });
        }
        return Task.FromResult(resChats);
    }
    
}