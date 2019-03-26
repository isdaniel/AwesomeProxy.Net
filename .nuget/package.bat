nuget pack ..\src\AwesomeProxy\AwesomeProxy.nuspec

::%%~nx 只取得檔名
for /f "delims=" %%a in ('dir /s /b *.nupkg') do set filename=%%~nxa

nuget push %filename%

del %filename%

Pause