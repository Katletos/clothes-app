﻿namespace Domain.Entities;

public class Review
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long UserId { get; set; }

    public short Rating { get; set; }

    public string Title { get; set; }

    public string Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; }

    public virtual User User { get; set; }
}