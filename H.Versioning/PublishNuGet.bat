call .\Release.bat

del /S /Q .\*nupkg

.\nuget.exe pack ".\H.Versioning.Cli\H.Versioning.Cli.csproj" -Prop Configuration=Release -IncludeReferencedProjects
.\nuget.exe push ".\H.Versioning.*.nupkg" 79ccf130-386b-41b8-a8e2-6ce17f0459ca -Source https://www.nuget.org/api/v2/package