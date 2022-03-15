
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

### Add Auto MFA auth:
Add a client MFA authentication, so this app can authenticate itself, 
without the user requiring to user their phones and entering a code at all.  
See:  
https://github.com/glacasa/TwoStepsAuthenticator#client-usage

### Add Code-signing
- https://stackoverflow.com/questions/252226/signing-a-windows-exe-file
- https://www.thesslstore.com/knowledgebase/code-signing-sign-code/sign-code-microsoft-authenticode/
- Makecert.exe https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-2.0/bfsktky3(v=vs.80)
- SignTool https://docs.microsoft.com/en-gb/windows/win32/seccrypto/signtool


