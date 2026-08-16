namespace Smartik.Features.MathTrainer.Models;

public sealed class MathExample(int firstOperand, int secondOperand, string @operator, int result) : ITrainerItem
{
    public int FirstOperand { get; } = firstOperand;
    public int SecondOperand { get; } = secondOperand;
    public string Operator { get; } = @operator;
    public int Result { get; } = result;

    public string? UserAnswer { get; set; } = string.Empty;
    public bool IsCorrect => int.TryParse(UserAnswer?.Trim(), out int val) && val == Result;
}
