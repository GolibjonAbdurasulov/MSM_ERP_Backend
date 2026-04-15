@echo off
echo === Frontend build ===
cd frontend
call npm run build
cd ..

echo === Backend publish ===
dotnet publish WebAPI/WebAPI.csproj -c Release -o ./publish/api

echo === React fayllarni ko'chirish ===
xcopy /E /Y /I frontend\dist publish\wwwroot\

echo === Production config ko'chirish ===
copy WebAPI\appsettings.Production.json publish\api\

echo === Zip qilish ===
powershell Compress-Archive -Path publish\* -DestinationPath msm-release.zip -Force

echo === Tayyor! msm-release.zip ===
pause