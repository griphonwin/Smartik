using Microsoft.Extensions.DependencyInjection;
using Photino.Blazor;
using Radzen;
using Smartik.Features.MathTrainer.Services;
using Smartik.Shared;
using System;
using System.IO;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // 1. Фикс рабочей директории
        string baseDir = AppContext.BaseDirectory;
        Directory.SetCurrentDirectory(baseDir); 

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

        // [КРИТИЧЕСКИЙ ФИКС ДЛЯ SINGLE FILE] 
        // Если движок не находит физический файл, мы говорим ему брать его из ресурсов сборки
        app.MainWindow.PathToFileReadyToRunDelegate = (string path) =>
        {
            return Path.Combine(baseDir, path);
        };

        // 4. НАСТРОЙКА ОКНА
        app.MainWindow
            .SetTitle("Smartik Math Trainer")
            .SetSize(1200, 800)
            .SetUseOsDefaultSize(false);

        // Иконку ставим безопасно
        string iconPath = Path.Combine(baseDir, "wwwroot", "icon.png");
        if (File.Exists(iconPath))
        {
            app.MainWindow.SetIconFile(iconPath);
        }
        
        app.MainWindow.LogVerbosity = 0;

        app.Run();
    }
}
