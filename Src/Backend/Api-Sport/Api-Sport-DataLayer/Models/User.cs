using System;
using System.Collections.Generic;

namespace Api_Sport_DataLayer.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Avatar { get; set; }

    public bool? IsBlock { get; set; }

    public string? Password { get; set; }
}
