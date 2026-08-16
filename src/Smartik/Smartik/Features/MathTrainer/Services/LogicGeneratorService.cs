using Smartik.Features.MathTrainer.Models;
using System.Security.Cryptography;

namespace Smartik.Features.MathTrainer.Services;

public sealed class LogicGeneratorService : ILogicGeneratorService
{
    public IReadOnlyList<LogicExample> GenerateLogicExamples(int count, int maxNumber)
    {
        var examples = new List<LogicExample>(count);
        // Хранилище уникальных ключей логических задач
        var uniqueKeys = new HashSet<string>(count);

        for (int i = 0; i < count; i++)
        {
            LogicExample? generatedExample = null;
            int attempts = 0;
            const int maxAttempts = 10;

            while (attempts < maxAttempts)
            {
                int left = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
                int right = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
                string correctSign = left < right ? "<" : (left > right ? ">" : "=");
                int position = RandomNumberGenerator.GetInt32(0, 3);

                if (position == 0)
                {
                    generatedExample = new LogicExample("_", right.ToString(), correctSign, left, 0);
                }
                else if (position == 2)
                {
                    generatedExample = new LogicExample(left.ToString(), "_", correctSign, right, 2);
                }
                else
                {
                    generatedExample = new LogicExample(left.ToString(), right.ToString(), correctSign, null, 1);
                }

                // Уникальный ключ логики учитывает операнды, знак и позицию скрытой клетки, например: "5<10_pos1"
                string key = $"{left}{correctSign}{right}_pos{position}";

                if (uniqueKeys.Add(key))
                {
                    break;
                }

                attempts++;
            }

            if (generatedExample != null)
            {
                examples.Add(generatedExample);
            }
        }

        return [.. examples];
    }
}
