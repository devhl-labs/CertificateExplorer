dotnet publish ..\..\src\CertificateExplorer\CertificateExplorer.csproj `
    -c Release `
    -o ..\..\src\CertificateExplorer\bin\Release\net5.0\win-x64\publish `
    -r win-x64 `
    -p:PublishReadyToRun=true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    --self-contained

dotnet publish ..\..\src\CertificateExplorer\CertificateExplorer.csproj `
    -c Release `
    -o ..\..\src\CertificateExplorer\bin\Release\net5.0\linux-x64\publish `
    -r linux-x64 `
    -p:PublishReadyToRun=false `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    --self-contained