using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;
using Radzen;
using Smartik.Features.MathTrainer.Services;
using Smartik.Shared;
using System.IO;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // 1. АВТОМАТИЧЕСКИЙ ФИКС ПАПКИ: Проверяем и создаем физическую папку wwwroot в bin, 
        // чтобы удовлетворить проверку движка Photino.Blazor
        string binWwwroot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
        if (!Directory.Exists(binWwwroot))
        {
            Directory.CreateDirectory(binWwwroot);
        }

        // 2. БЕЗОПАСНЫЙ ПЕРЕКЛЮЧАТЕЛЬ РЕЖИМА: Отключаем InteractiveServer в основном App.razor
        Smartik.Shared.App.IsDesktopMode = true;

        // 3. ИНИЦИАЛИЗАЦИЯ ДВИЖКА PHOTINO BLAZOR
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        // РЕГИСТРАЦИЯ СЕРВИСОВ ТРЕНАЖЕРА
        appBuilder.Services.AddRadzenComponents();
        appBuilder.Services.AddScoped<IMathGeneratorService, MathGeneratorService>();
        appBuilder.Services.AddScoped<ILogicGeneratorService, LogicGeneratorService>();
        appBuilder.Services.AddScoped<IPrintService, PrintService>();
        appBuilder.Services.AddLogging();

        // Монтируем локальный App десктопного проекта
        appBuilder.RootComponents.Add<App>("#app");

        var app = appBuilder.Build();

        // НАСТРОЙКА ОКНА
        app.MainWindow
            .SetTitle("Smartik Math Trainer (Blazor Hybrid)")
            .SetSize(1200, 800)
            .SetUseOsDefaultSize(false);

        // ЗАПУСК ГИБРИДНОГО ПРИЛОЖЕНИЯ
        app.Run();
    }
}
