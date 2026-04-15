@echo off

echo === Backend papkaga o'tish ===
cd /d "%~dp0"

echo === Eski publish papkani tozalash ===
if exist publish rd /S /Q publish

echo === Backend publish ===
dotnet publish WebAPI/WebAPI.csproj -c Release -o ./publish/api
if errorlevel 1 (
    echo XATO: Backend publish muvaffaqiyatsiz!
    pause
    exit /b 1
)

echo === Production config ko'chirish ===
copy WebAPI\appsettings.Production.json publish\api\

echo === Zip qilish ===
if exist msm-backend.zip del msm-backend.zip
powershell Compress-Archive -Path publish\* -DestinationPath msm-backend.zip -Force

echo ================================
echo   msm-backend.zip tayyor!
echo ================================
pause