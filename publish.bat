@echo off
set ENV=linux-arm
set WORK_DIR=%~dp0
set RAR_DIR=%ProgramFiles%
echo publish ENV=%ENV%
del %WORK_DIR%publish.zip
dotnet publish -c Release -r %ENV%
cd /d %RAR_DIR%\WinRAR
WinRAR a -ep1 %WORK_DIR%publish.zip %WORK_DIR%bin\%ENV%\publish
rd /q /s %WORK_DIR%bin
echo done.
ping -n 3 127.1 >nul
