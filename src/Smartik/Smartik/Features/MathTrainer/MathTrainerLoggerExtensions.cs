namespace Smartik.Features.MathTrainer;

public static partial class MathTrainerLoggerExtensions
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Сгенерировано {Count} примеров на '{Operation}' с максимальным числом {MaxNumber}")]
    public static partial void LogExamplesGenerated(this ILogger logger, int count, string operation, int maxNumber);
}
