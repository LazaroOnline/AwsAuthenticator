
cd ..
$publishFolder = "Publish"
$publishFolderApp = "$publishFolder/app"

# Cleanup:
Write-Host -F Blue "Cleaning up folders: '$publishFolderApp' and '$publishFolderDotnetTool'..."
if (test-path $publishFolderApp       ) { Remove-Item "$publishFolderApp/*"        -Force -Recurse; Write-Host -F Yellow "Removed folder: $publishFolderApp" }
Write-Host -F Blue "Starting compilation..."

# COMPILE AS EXE:
# https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
Write-Host -F Blue "Publishing app..."
dotnet publish "src/AwsAuthenticator.UI/AwsAuthenticator.UI.csproj" -c "Release" -o $publishFolderApp /p:DebugType=None -p:PublishSingleFile=true --self-contained false # -r "win-x64"
Write-Host -F Green "Publishing app DONE!"

$version = (Get-Item "$publishFolderApp/AwsAuthenticator.exe").VersionInfo.FileVersion


$destinationZip = "$publishFolder/AwsAuthenticator-v$version.zip"
Write-Host -F Blue "Compressing binaries into: '$destinationZip'..."
# https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.archive/compress-archive?view=powershell-7.3
Compress-Archive -Path "$publishFolderApp/*" -DestinationPath $destinationZip -Force

Start $publishFolder

Write-Host -F Green "Finished Build!"
