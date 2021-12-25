
# User's Guide


## Requirements
- [Aws CLI](https://aws.amazon.com/cli/).
- [DotNet 6.0 Runtime](https://dotnet.microsoft.com/download).
- Windows (Currently only Windows is supported, but it should be easy to add support for Mac and Linux).


## How to use
Open the app, enter the required fields and click on the "Update credentials" button,
this will edit your local aws credentials file at:  
`C:\Users\%USERNAME%\.aws\credentials`  

Editing the following fields, from the selected profile, with the new values from the command `aws sts get-session-token`:
- `aws_access_key_id`
- `aws_secret_access_key`
- `aws_session_token`


### UI Mode
Just running the app will show the UI with a form to fill in all required configuration.  
The configuration is automatically saved in the `AppSettings.json` file when the app closes.


### Console Mode
This app can be used from the command-line, 
if certain command-line parameters are found like the `--Aws:TokenCode` parameter, the app will run in console mode instead of launching the UI.
Example:  
```cmd
AwsCredentialManager.exe --Aws:AccountId 670441985101 --Aws:UserName YOURUSER@YOURCOMPANY.com --Aws:Profile opsmfa --Aws:TokenCode 123456
# Or just:
AwsCredentialManager.exe -A 670441985101 -U YOURUSER@YOURCOMPANY.com -P opsmfa -C 123456
```
If the app is already configured in the `AppSettings.json` file, then you only need to specify the `Token` parameter:
```cmd
AwsCredentialManager.exe -Token 123456 
```

## Configuration
The configuration is stored in a file named `AppSettings.json` placed next to the application.  

 Config parameter    | Short Name | Description
---------------------|------------|---------------------------------------------------------------------------------
 Aws > AccountId     |    -A      | 12 digit number of the AWS project account.
 Aws > UserName      |    -U      | Personal AWS user name, usually your email.
 Aws > Profile       |    -P      | Aws profile name. It should already exist in your aws credentials file.
 Aws > Token         | -T or -C   | MFA personal user token (one time token).

