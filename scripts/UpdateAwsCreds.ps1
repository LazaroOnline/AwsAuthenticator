
# Script by Alberto Corrales:

$MfaDeviceArn = "#YOUR_MFA_DEVICE_ARN#"
$DurationInSeconds = 28800
$OpsProfileName = "ops"
$MfaProfileName = "mfadev"

$MfaCode = Read-Host -Prompt 'Please, input your MFA code'
$TempCredentials = aws sts get-session-token --duration-seconds $DurationInSeconds --serial-number $MfaDeviceArn --token-code $MfaCode --profile $OpsProfileName  | ConvertFrom-Json 
Write-Output "Updating your profile '$MfaProfileName' with your new temporary credentials..."
$AccessKeyId = ($TempCredentials).Credentials.AccessKeyId
$SecretAccessKey = ($TempCredentials).Credentials.SecretAccessKey
$SessionToken = ($TempCredentials).Credentials.SessionToken
aws configure set aws_access_key_id $AccessKeyId --profile $MfaProfileName
aws configure set aws_secret_access_key $SecretAccessKey --profile $MfaProfileName
aws configure set aws_session_token $SessionToken --profile $MfaProfileName

Write-Output "Profile '$MfaProfileName' updated successfully!"
Read-Host  -Prompt "Press any key to continue or CTRL+C to quit" 
