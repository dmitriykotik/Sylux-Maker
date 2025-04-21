@echo off
cd make
dotnet publish -c Release -r linux-x64 -o ./publish -p:PublishSingleFile=true
pause