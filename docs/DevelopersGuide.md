
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

### Add Copy button next to the Token-Code:
There may be a class to do it using `Classes="..."` property.
References:  
- [Avalonia TextBox docs](https://docs.avaloniaui.net/docs/controls/textbox) 
- [Avalonia TextBox.xaml source](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Themes.Fluent/Controls/TextBox.xaml#L5) 


### Hide the MFA Device Generator secret key
Currently it is stored in the `AppSettings.json` file as `MfaGeneratorSecretKey`.  
Maybe it would be better secured using some kind of Windows-Credentials-Manager API, 
or at least encoded in some way to add obfuscation.


### Add Code-signing
- https://stackoverflow.com/questions/252226/signing-a-windows-exe-file
- https://www.thesslstore.com/knowledgebase/code-signing-sign-code/sign-code-microsoft-authenticode/
- Makecert.exe https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-2.0/bfsktky3(v=vs.80)
- SignTool https://docs.microsoft.com/en-gb/windows/win32/seccrypto/signtool

