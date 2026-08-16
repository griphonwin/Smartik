namespace Smartik.Features.MathTrainer.Models;

public interface ITrainerItem
{
    string? UserAnswer { get; set; }
    bool IsCorrect { get; }
}
