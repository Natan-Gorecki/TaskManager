﻿using System.ComponentModel.DataAnnotations;

namespace TaskManager.Service.Database.Models;

internal class DbBase
{
    [Key]
    public required string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
