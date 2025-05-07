using System;
using System.Collections.Generic;

namespace Api_Sport_DataLayer_DataLayer.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int MatchesId { get; set; }

    public int UserId { get; set; }

    public string? Text { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Match Matches { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
