using Smartik.Features.MathTrainer.Models;
using System.Security.Cryptography;
namespace Smartik.Features.MathTrainer.Services;

public sealed class MathGeneratorService : BaseGeneratorService<MathExample, MathGeneratorService.ExampleCandidate, long>, IMathGeneratorService
{
    public readonly record struct ExampleCandidate(int First, int Second, bool IsAddition, int Result);

    private int _zeroCount;
    private int _oneCount;

    public IReadOnlyList<MathExample> GenerateMixedExamples(int count, int maxNumber)
        => GenerateExamples(count, maxNumber); // Просто вызываем базовый каркас

    protected override void ResetSessionState()
    {
        _zeroCount = 0;
        _oneCount = 0;
    }

    protected override ExampleCandidate GenerateCandidate(int maxNumber)
    {
        bool isAddition = RandomNumberGenerator.GetInt32(0, 2) == 0;
        return isAddition ? GenerateAddition(maxNumber) : GenerateSubtraction(maxNumber);
    }

    protected override bool IsValidCandidate(in ExampleCandidate candidate, HashSet<long> uniqueKeys, out long key)
    {
        key = 0;

        if (candidate.First == candidate.Second) return false;

        int zeros = (candidate.First == 0 ? 1 : 0) + (candidate.Second == 0 ? 1 : 0);
        int ones = (candidate.First == 1 ? 1 : 0) + (candidate.Second == 1 ? 1 : 0);

        if (_zeroCount + zeros > 1 || _oneCount + ones > 1) return false;

        // Вычисляем ключ
        int first = candidate.First;
        int second = candidate.Second;
        if (candidate.IsAddition && first > second)
        {
            (first, second) = (second, first);
        }
        long opBit = candidate.IsAddition ? 1L : 0L;
        key = ((long)first << 33) | ((long)second << 1) | opBit;

        return !uniqueKeys.Contains(key);
    }

    protected override void OnExampleAdded(in ExampleCandidate candidate)
    {
        _zeroCount += (candidate.First == 0 ? 1 : 0) + (candidate.Second == 0 ? 1 : 0);
        _oneCount += (candidate.First == 1 ? 1 : 0) + (candidate.Second == 1 ? 1 : 0);
    }

    protected override MathExample CreateFinalExample(in ExampleCandidate candidate)
    {
        string opString = candidate.IsAddition ? "+" : "-";
        return new MathExample(candidate.First, candidate.Second, opString, candidate.Result);
    }

    // Приватные методы генерации (GenerateAddition / GenerateSubtraction) остаются без изменений...
    private static ExampleCandidate GenerateAddition(int maxNumber)
    {
        int result = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
        int first = RandomNumberGenerator.GetInt32(0, result + 1);
        int second = result - first;
        if (maxNumber == 20 && (first > 10 || second > 10))
        {
            if ((first % 10) + (second % 10) > 9)
            {
                first = RandomNumberGenerator.GetInt32(10, 16);
                second = RandomNumberGenerator.GetInt32(0, 5);
                result = first + second;
            }
        }
        return new ExampleCandidate(first, second, IsAddition: true, result);
    }

    private static ExampleCandidate GenerateSubtraction(int maxNumber)
    {
        int first = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
        int second = RandomNumberGenerator.GetInt32(0, first + 1);
        int result = first - second;
        if (maxNumber == 20 && first > 10)
        {
            if ((first % 10) < (second % 10))
            {
                second = RandomNumberGenerator.GetInt32(0, (first % 10) + 1);
                result = first - second;
            }
        }
        return new ExampleCandidate(first, second, IsAddition: false, result);
    }
}