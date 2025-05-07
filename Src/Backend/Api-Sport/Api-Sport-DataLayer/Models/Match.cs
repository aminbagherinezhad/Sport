using System;
using System.Collections.Generic;

namespace Api_Sport_DataLayer_DataLayer.Models;

public partial class Match
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Text { get; set; }

    public string? ImageName { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
}
