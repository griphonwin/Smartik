using Smartik.Features.MathTrainer.Models;
using System.Security.Cryptography;

namespace Smartik.Features.MathTrainer.Services;

public sealed class LogicGeneratorService : BaseGeneratorService<LogicExample, LogicGeneratorService.LogicCandidate, long>, ILogicGeneratorService
{
    public readonly record struct LogicCandidate(int Left, int Right, int Position);

    public IReadOnlyList<LogicExample> GenerateLogicExamples(int count, int maxNumber)
        => GenerateExamples(count, maxNumber);

    protected override void ResetSessionState() { } // Логике пока не нужны счетчики состояния

    protected override LogicCandidate GenerateCandidate(int maxNumber)
    {
        int left = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
        int right = RandomNumberGenerator.GetInt32(0, maxNumber + 1);
        int position = RandomNumberGenerator.GetInt32(0, 3);
        return new LogicCandidate(left, right, position);
    }

    protected override bool IsValidCandidate(in LogicCandidate candidate, HashSet<long> uniqueKeys, out long key)
    {
        key = ((long)candidate.Left << 35) | ((long)candidate.Right << 3) | (long)candidate.Position;
        return !uniqueKeys.Contains(key);
    }

    protected override void OnExampleAdded(in LogicCandidate candidate) { }

    protected override LogicExample CreateFinalExample(in LogicCandidate candidate)
    {
        string correctSign = candidate.Left < candidate.Right ? "<" : (candidate.Left > candidate.Right ? ">" : "=");

        if (candidate.Position == 0)
            return new LogicExample("_", candidate.Right.ToString(), correctSign, candidate.Left, 0);

        if (candidate.Position == 2)
            return new LogicExample(candidate.Left.ToString(), "_", correctSign, candidate.Right, 2);

        return new LogicExample(candidate.Left.ToString(), candidate.Right.ToString(), correctSign, null, 1);
    }
}
