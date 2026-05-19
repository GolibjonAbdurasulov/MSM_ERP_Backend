using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

[Table("telegram_chats")]
public class TelegramChat : BaseEntity<long>
{
    [Required]
    [Column("chat_id")]
    public long ChatId { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("username")]
    public string? Username { get; set; }

    [Column("chat_type")]
    public string ChatType { get; set; } = "group";
    
    [Column("bot_added")]
    public bool BotAdded { get; set; }

    [Column("can_send_message")]
    public bool CanSendMessage { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}