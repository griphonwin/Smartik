using Smartik.Features.MathTrainer.Models;

namespace Smartik.Features.MathTrainer.Services;

public interface ILogicGeneratorService
{
    IReadOnlyList<LogicExample> GenerateLogicExamples(int count, int maxNumber);
}
