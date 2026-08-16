namespace Smartik.Features.MathTrainer.Models;

public sealed class LogicExample(string leftText, string rightText, string correctSign, int? missingValue = null, int? hiddenPosition = null) : ITrainerItem
{
    public string LeftText { get; } = leftText;
    public string RightText { get; } = rightText;
    public string CorrectSign { get; } = correctSign;
    public int? MissingValue { get; } = missingValue;
    public int? HiddenPosition { get; } = hiddenPosition; // 0 - слева, 1 - знак, 2 - справа

    public string? UserAnswer { get; set; } = string.Empty;

    public bool IsCorrect
    {
        get
        {
            if (string.IsNullOrWhiteSpace(UserAnswer)) return false;
            var cleanAnswer = UserAnswer.Trim();

            // Ситуация 1: Ребёнок подставлял знак сравнения по центру
            if (HiddenPosition == 1)
            {
                return cleanAnswer == CorrectSign;
            }

            // Ситуация 2: Ребёнок подставлял пропущенное число (слева или справа)
            if (!int.TryParse(cleanAnswer, out int userNum)) return false;

            // Парсим левое и правое числа, учитывая, где был пропуск
            int leftNum = HiddenPosition == 0 ? userNum : int.Parse(LeftText);
            int rightNum = HiddenPosition == 2 ? userNum : int.Parse(RightText);

            // Честно вычисляем математическое неравенство на бэкенде!
            return CorrectSign switch
            {
                "<" => leftNum < rightNum,
                ">" => leftNum > rightNum,
                "=" => leftNum == rightNum,
                _ => false
            };
        }
    }
}
