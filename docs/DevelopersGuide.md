
# Developer's Guide

## Requirements
- [Aws CLI](https://aws.amazon.com/cli/).
- [Dotnet 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- Visual Studio 2022 or higher.
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
  - `AWS Authenticator`
  - `AWS MFA Authenticator`
  - `AWS-MFA`
  - `PC-MFA`
  - `AutoMfa`
  - `AwsAutoMfa`
  - `PcAuthenticator`
  - `PcMultiFactorAuthenticator`
- Show how long since last updated AWS credentials, monitor the file.
- Add a button to configure the automatic run of this program from Windows Tasks.
- Show the countdown timer displaying until when the token is valid (not currently supported by the MFA NuGet library).  
  The countdown is 30 secs, tokens are created at the beginning of each minute from .00 to .30 secs, and from .30 to .00 secs.
  A timer can be implemented to auto-renovate the token at the correct time.
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



### Adding automatic "Access-keys" renewal:
Feature proposal to add a button to update the API key in the AWS config file, 
when it expires after 30 days (depending on aws account policy).  
The command `aws iam list-access-keys`can be used to get the `CreateDate` of the keys used in the "default" profile config, 
and determine if it should update the keys with `aws iam create-access-key`.  
- https://awscli.amazonaws.com/v2/documentation/api/latest/reference/iam/list-access-keys.html
- https://awscli.amazonaws.com/v2/documentation/api/latest/reference/iam/create-access-key.html
- https://docs.aws.amazon.com/cli/latest/userguide/cli-services-iam-create-creds.html
```
aws iam create-access-key
aws iam create-access-key --user-name your.name@company.com
```

Response example:
```json
{
    "AccessKey": {
        "UserName": "personal-aws-cli",
        "AccessKeyId": "AKIAAAAAAAAAAAAAAAAA",
        "Status": "Active",
        "SecretAccessKey": "SOME_EXAMPLE_KEY______________EXAMPLEKEY",
        "CreateDate": "2023-01-00T00:00:00Z"
    }
}
```
The problem is that the user needs to have permissions to execute the CLI command 
`aws iam create-access-key`, even if he has permissions to do so in the AWS web console.
If no permissions the command returns this error:
```
An error occurred (AccessDenied) when calling the CreateAccessKey operation: 
User: arn:aws:iam::123456789000:user/your.name@company.com is not authorized to perform: iam:CreateAccessKey 
on resource: user your.name@company.com with an explicit deny in an identity-based policy
```
