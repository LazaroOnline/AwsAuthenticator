
# Developer''s Guide

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
