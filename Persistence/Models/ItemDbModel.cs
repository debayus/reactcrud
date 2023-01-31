using System;
namespace Persistence.Models;

public class ItemDbModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
}

