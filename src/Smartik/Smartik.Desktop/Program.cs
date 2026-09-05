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
        // 1. Фикс папки
        string baseDir = AppContext.BaseDirectory;
        // Указываем Photino, что wwwroot нужно искать там, куда распаковался .exe
        Directory.SetCurrentDirectory(baseDir); 
        string binWwwroot = Path.Combine(baseDir, "wwwroot");
        if (!Directory.Exists(binWwwroot)) Directory.CreateDirectory(binWwwroot);        

        // 2. Отключаем InteractiveServer в веб-версии
        Smartik.Shared.App.IsDesktopMode = true;

        // 3. Инициализация Photino
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.Services.AddRadzenComponents();
        appBuilder.Services.AddScoped<IMathGeneratorService, MathGeneratorService>();
        appBuilder.Services.AddScoped<ILogicGeneratorService, LogicGeneratorService>();
        appBuilder.Services.AddScoped<IPrintService, PrintService>();
        appBuilder.Services.AddLogging();

        appBuilder.RootComponents.Add<App>("#app");

        var app = appBuilder.Build();

        // 4. НАСТРОЙКА ОКНА
        app.MainWindow
            .SetTitle("Smartik Math Trainer")
            .SetSize(1200, 800)
            .SetUseOsDefaultSize(false);

        string iconPath = Path.Combine(baseDir, "wwwroot", "icon.png");
        if (File.Exists(iconPath))
        {
            app.MainWindow.SetIconFile(iconPath);
        }
        app.MainWindow.LogVerbosity = 0;

        app.Run();
    }
}
