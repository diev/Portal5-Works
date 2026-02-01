namespace CryptoBot.Models;

public class TaskSpecial
{
    public string TaskName { get; set; } = nameof(Program);
    public string[] Subscribers { get; set; } = [];
}
