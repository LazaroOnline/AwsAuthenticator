
# Developer's Guide

## Requirements
- [Aws CLI](https://aws.amazon.com/cli/).
- [Dotnet 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
- Visual Studio 2019 or higher.
- Visual Studio [add-on for Avalonia UI](https://marketplace.visualstudio.com/items?itemName=AvaloniaTeam.AvaloniaVS).


## Other UI frameworks
The initial version was created with Avalonia for being the option that offers faster development in the familiar C# language with multi OS support, basically Windows/MacOS/Linux and command-line support.  
Other UI options are:
- [Flutter](https://flutter.dev/multi-platform/desktop): it is mobile first, not desktop first. High learning curve, requires Dart language.
- [React Native for Windows](https://microsoft.github.io/react-native-windows/).
- Dotnet MAUI: currently only in beta in 2021.
```PS
dotnet new -i Microsoft.Maui.Templates
dotnet new maui
```

## Future work
- Rename the project to something like one of these names:
  - `PC-MFA`
  - `AutoMfa`
  - `AwsAutoMfa`
  - `AwsAuthenticator`
  - `PcAuthenticator`
  - `PcMultiFactorAuthenticator`
- Show how long since last updated AWS credentials, monitor the file.
- Add a button to configure the automatic run of this program from Windows Tasks.
- Show the countdown timer displaying until when the token is valid (not currently supported by the MFA NuGet library).  
  The countdown is 30 secs, tokens are created at the beginning of each minute from .00 to .30 secs, and from .30 to .00 secs.
  A timer can be implemented to auto-renovate the token at the correct time.
- Add a button to update the API key in the AWS config file, when it expires after 30 days (depending on aws account policy).
- Remember the window size and position after re-opening the app.  
- Hide the MFA Device Generator secret key:  
    Currently it is stored in the `AppSettings.json` file as `MfaGeneratorSecretKey`.  
    Maybe it would be better secured using some kind of Windows-Credentials-Manager API, 
    or at least encoded in some way to add obfuscation.
- Add Code-signing to the app.
  * https://stackoverflow.com/questions/252226/signing-a-windows-exe-file
  * https://www.thesslstore.com/knowledgebase/code-signing-sign-code/sign-code-microsoft-authenticode/
  * Makecert.exe https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-2.0/bfsktky3(v=vs.80)
  * SignTool https://docs.microsoft.com/en-gb/windows/win32/seccrypto/signtool

