using Microsoft.JSInterop;

namespace Smartik.Features.MathTrainer.Services;

public sealed class PrintService(IJSRuntime jsRuntime) : IPrintService
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public async Task TriggerPrintAsync()
    {
        try
        {
            // Напрямую вызываем нативную печать браузера через наш сервис
            await _jsRuntime.InvokeVoidAsync("window.mathTrainer.triggerPrint");
        }
        catch
        {
            // Защита на случай обрыва соединения SignalR
        }
    }
}
