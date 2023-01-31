using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class ItemModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
}

public class ItemPostPutModel
{
    [Required]
    public string Title { get; set; } = default!;
}

