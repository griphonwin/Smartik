using Smartik.Features.MathTrainer.Models;
using System.Security.Cryptography;

namespace Smartik.Features.MathTrainer.Services;

public sealed class MathGeneratorService : IMathGeneratorService
{
    public IReadOnlyList<MathExample> GenerateMixedExamples(int count, int maxNumber)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfNegative(maxNumber);

        var examples = new List<MathExample>(count);
        // Хранилище уникальных ключей для текущего билета
        var uniqueKeys = new HashSet<string>(count);

        for (int i = 0; i < count; i++)
        {
            MathExample? generatedExample = null;
            int attempts = 0;
            const int maxAttempts = 10; // Лимит в 10 попыток перегенерации

            while (attempts < maxAttempts)
            {
                bool isAddition = RandomNumberGenerator.GetInt32(0, 2) == 0;

                if (isAddition)
                {
                    int result = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
                    int first = RandomNumberGenerator.GetInt32(0, result + 1);
                    int second = result - first;

                    if (maxNumber == 20 && (first > 10 || second > 10))
                    {
                        int firstUnits = first % 10;
                        int secondUnits = second % 10;
                        if (firstUnits + secondUnits > 9)
                        {
                            first = RandomNumberGenerator.GetInt32(10, 16);
                            second = RandomNumberGenerator.GetInt32(0, 5);
                            result = first + second;
                        }
                    }

                    generatedExample = new MathExample(first, second, "+", result);
                }
                else
                {
                    int first = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
                    int second = RandomNumberGenerator.GetInt32(0, first + 1);
                    int result = first - second;

                    if (maxNumber == 20 && first > 10)
                    {
                        int firstUnits = first % 10;
                        int secondUnits = second % 10;
                        if (firstUnits < secondUnits)
                        {
                            second = RandomNumberGenerator.GetInt32(0, firstUnits + 1);
                            result = first - second;
                        }
                    }

                    generatedExample = new MathExample(first, second, "-", result);
                }

                // Формируем уникальный ключ примера, например: "5+3" или "10-4"
                string key = $"{generatedExample.FirstOperand}{generatedExample.Operator}{generatedExample.SecondOperand}";

                // Если ключ уникален — прерываем цикл перегенерации и оставляем пример
                if (uniqueKeys.Add(key))
                {
                    break;
                }

                attempts++;
            }

            // Добавляем пример (даже если лимит попыток исчерпан, чтобы не сломать размер сетки)
            if (generatedExample != null)
            {
                examples.Add(generatedExample);
            }
        }

        return [.. examples];
    }

    public IReadOnlyList<MathExample> GenerateAdditionExamples(int count, int maxNumber) =>
        [.. GenerateMixedExamples(count, maxNumber).Where(e => e.Operator == "+")];

    public IReadOnlyList<MathExample> GenerateSubtractionExamples(int count, int maxNumber) =>
        [.. GenerateMixedExamples(count, maxNumber).Where(e => e.Operator == "-")];
}
