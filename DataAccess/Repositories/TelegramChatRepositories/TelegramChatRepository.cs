using Core.Attributes;
using DataAccess.DataContext;
using DataAccess.Entities;
using DataAccess.Repositories.BaseRepository;

namespace DataAccess.Repositories.TelegramChatRepositories;

[Injectable]
public class TelegramChatRepository(AppDbContext dbContext)
    : RepositoryBase<TelegramChat, long>(dbContext), ITelegramChatRepository;