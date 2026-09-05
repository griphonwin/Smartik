# Очищаем консоль для красоты
Clear-Host
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "     СБОРКА SMARTIK DESKTOP В ОДИН .EXE ФАЙЛ      " -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan

# Настройка путей относительно корня решения
$ProjectPath = ".\Smartik.Desktop\Smartik.Desktop.csproj"
$OutputFolder = ".\build\Stepik"

# 1. Очистка целевой папки, чтобы там не оставалось старых файлов
if (Test-Path $OutputFolder) {
    Write-Host "1. Очистка старой папки сборки..." -ForegroundColor Yellow
    Remove-Item -Path "$OutputFolder\*" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "1. Создание папки для сборки..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $OutputFolder | Out-Null
}

Write-Host "2. Запуск компиляции и упаковки в Single File (.NET 10)..." -ForegroundColor Yellow

# Запускаем публикацию с флагом -o (Output), принудительно перенаправляя всё в вашу папку
dotnet publish $ProjectPath `
    -c Release `
    -r win-x64 `
    -o $OutputFolder `
    --self-contained true `
    /p:PublishSingleFile=true `
    /p:PublishReadyToRun=true `
    /p:IncludeNativeLibrariesForSelfExtract=true `
    /p:EnableCompressionInSingleFile=true

# Проверяем, успешно ли отработал компилятор .NET
if ($LASTEXITCODE -eq 0) {
    Write-Host "`n==================================================" -ForegroundColor Cyan
    Write-Host " СБОРКА УСПЕШНО ЗАВЕРШЕНА!" -ForegroundColor Green
    Write-Host " Готовый .exe файл находится здесь: $OutputFolder" -ForegroundColor Green
    Write-Host "==================================================" -ForegroundColor Cyan
    
    # Автоматически открываем вашу папку в проводнике Windows
    explorer.exe $OutputFolder
} else {
    Write-Host "`n[ОШИБКА] Не удалось собрать десктопное приложение!" -ForegroundColor Red
}

Write-Host "`nНажмите любую клавишу, чтобы закрыть окно..."
[System.Console]::ReadKey() | Out-Null
