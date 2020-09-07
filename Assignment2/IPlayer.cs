using System;
public interface IPlayer
{
    int Score { get; set; }
    public Guid Id { get; set; }
}