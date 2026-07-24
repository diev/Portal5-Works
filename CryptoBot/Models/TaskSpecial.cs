namespace CryptoBot.Models;

public class TaskSpecial
{
    public string TaskName { get; set; } = nameof(Program);
    public List<string> Subscribers { get; set; } = [];
}
