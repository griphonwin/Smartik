using Smartik.Features.MathTrainer.Models;

namespace Smartik.Features.MathTrainer.Services;

public interface IMathGeneratorService
{
    IReadOnlyList<MathExample> GenerateMixedExamples(int count, int maxNumber);
}
