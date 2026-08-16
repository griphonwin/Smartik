using Smartik.Features.MathTrainer.Models;

namespace Smartik.Features.MathTrainer.Services;

public interface IMathGeneratorService
{
    IReadOnlyList<MathExample> GenerateMixedExamples(int count, int maxNumber);
    IReadOnlyList<MathExample> GenerateAdditionExamples(int count, int maxNumber);
    IReadOnlyList<MathExample> GenerateSubtractionExamples(int count, int maxNumber);
}
