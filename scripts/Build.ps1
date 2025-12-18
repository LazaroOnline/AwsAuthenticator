# BUILD APP

# Navitage to the solution folder:
$scriptFolder = if ($PSScriptRoot -eq "") { "." } else { $PSScriptRoot } # Support PSv5/Core running from script file or copy/pasted into a prompt.
cd "$scriptFolder/.."
Write-Host -F Blue "Changed dir to: '$PWD'"


# CONFIG:
$csprojPath = "src/AwsAuthenticator.UI/AwsAuthenticator.UI.csproj"
$appFileName = "AwsAuthenticator" # .exe
$publishFolder = "Publish"
$publishFolderApp = "$publishFolder/app"


# CLEANUP:
Write-Host -F Blue "Cleaning up folders: '$publishFolderApp' and '$publishFolderDotnetTool'..."
if (test-path $publishFolderApp) { Remove-Item "$publishFolderApp/*" -Force -Recurse; Write-Host -F Yellow "Removed folder: $publishFolderApp" }
Write-Host -F Blue "Starting compilation..."


# COMPILE AS EXE:
# https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
Write-Host -F Blue "Publishing app..."
dotnet publish $csprojPath `
--self-contained false `
-o $publishFolderApp `
-c "Release" `
-p:DebugType=None `
-p:EnableCompressionInSingleFile=false `
-p:PublishAot=false `
-p:PublishTrimmed=false `
-p:PublishSingleFile=true 
# Enabling "PublishTrimmed" trimming works but breaks the "Jot" library used in "WindowStateTracker.cs" to remember the window position during restarts, and throws warnings for "ReactiveUI.WhenAnyMixin.WhenAnyValue<>()".
# -r "win-x64" 
$compiledAppFile = Get-Item "$publishFolderApp/$appFileName.exe" -ErrorAction SilentlyContinue
if ($compiledAppFile -eq $null) {
	Write-Host -F Red "Error compiling/publishing ('$appFileName.exe' not created)."
	return
}
Write-Host -F Green "Publishing app DONE!"
return

# COMPRESS TO ZIP:
$version = $compiledAppFile.VersionInfo.FileVersion
$destinationZip = "$publishFolder/$appFileName-v$version.zip"
Write-Host -F Blue "Compressing binaries into: '$destinationZip'..."
# https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.archive/compress-archive?view=powershell-7.3
Compress-Archive -Path "$publishFolderApp/*" -DestinationPath $destinationZip -Force


Start $publishFolder
Write-Host -F Green "Finished Build!"
