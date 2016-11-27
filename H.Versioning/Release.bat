echo off

rmdir .\Build\bin\Release /S /Q
rmdir .\Build\Publish /S /Q

"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" ".\H.Versioning\H.Versioning.csproj" /t:Build /p:Configuration=Release /verbosity:minimal
if errorlevel 1 exit

"C:\Program Files (x86)\MSBuild\14.0\Bin\MsBuild.exe" ".\H.Versioning.Cli\H.Versioning.Cli.csproj" /t:Build /p:Configuration=Release /verbosity:minimal
if errorlevel 1 exit

robocopy .\H.Versioning\bin\Release .\Build\Publish *.dll /s
robocopy .\H.Versioning.Cli\bin\Release .\Build\Publish *.dll /s
robocopy .\H.Versioning.Cli\bin\Release .\Build\Publish *.exe /s

.\Build\Publish\H.Versioning.Cli.exe >> .\Build\Publish\version.txt

del /S /Q .\Build\Publish\*.vshost.*
ren .\Build\Publish\*.config *.config.new
ren .\Build\Publish\CommandRunner\*.config *.config.new
del /S /Q .\Build\HsVersioningRelease.zip
7z\7z.exe a .\Build\HsVersioningRelease.zip .\Build\Publish\*