using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.TelegramChatRepositories;

public interface ITelegramChatRepository : IRepositoryBase<TelegramChat,long>
{
    
}