using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebAPI.Common;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TelegramChatController : ControllerBase
{
    private readonly ITelegramChatService _telegramChatService;

    public TelegramChatController(ITelegramChatService telegramChatService)
    {
        _telegramChatService = telegramChatService;
    }

    [HttpGet]
    public async Task<ResponseModelBase> GetAllTelegramChats()
    {
        var res= await _telegramChatService.GetAllTelegramChats();
        return (res,200);
    }
}