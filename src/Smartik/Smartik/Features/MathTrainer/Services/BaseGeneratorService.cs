using System.Security.Cryptography;

namespace Smartik.Features.MathTrainer.Services;

public abstract class BaseGeneratorService<TExample, TCandidate, TKey>
    where TKey : notnull
{
    protected abstract TCandidate GenerateCandidate(int maxNumber);
    protected abstract bool IsValidCandidate(in TCandidate candidate, HashSet<TKey> uniqueKeys, out TKey key);
    protected abstract TExample CreateFinalExample(in TCandidate candidate);
    protected abstract void OnExampleAdded(in TCandidate candidate);
    protected abstract void ResetSessionState();

    public IReadOnlyList<TExample> GenerateExamples(int count, int maxNumber)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfNegative(maxNumber);

        var examples = new List<TExample>(count);
        var uniqueKeys = new HashSet<TKey>(count);

        ResetSessionState(); // Сбрасываем счетчики 0, 1 и т.д. перед новой генерацией

        for (int i = 0; i < count; i++)
        {
            int attempts = 0;
            const int maxAttempts = 50;
            bool isAddedOnThisStep = false;

            while (attempts < maxAttempts)
            {
                // 1. Шаг генерации (специфичный для каждого сервиса)
                TCandidate candidate = GenerateCandidate(maxNumber);

                // 2. Шаг валидации (у каждого свои правила и свой тип ключа)
                if (IsValidCandidate(in candidate, uniqueKeys, out TKey key))
                {
                    uniqueKeys.Add(key);

                    // Уведомляем наследника (например, для обновления счетчиков нулей/единиц)
                    OnExampleAdded(in candidate);

                    // 3. Создание финального тяжелого объекта
                    examples.Add(CreateFinalExample(in candidate));

                    isAddedOnThisStep = true;
                    break;
                }

                attempts++;
            }

            if (!isAddedOnThisStep)
            {
                i--; // Безопасный откат
            }
        }

        return [.. examples];
    }
}
