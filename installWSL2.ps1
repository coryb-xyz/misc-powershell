$dismArgs = @(
    "/online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart"
    "/online /enable-feature /featurename:VirtualMachinePlatform /all /norestart"
)

foreach ($arg in $dismArgs) {
    Start-Process "dism.exe" -Wait -ArgumentList $arg
}

$msiUri = "https://wslstorestorage.blob.core.windows.net/wslblob/wsl_update_x64.msi"
$msiDesitnation = "~/Downloads/wsl_update_x64.msi"

Invoke-WebRequest -Uri $msiUri -OutFile $msiDesitnation

if (Test-Path $msiDesitnation) {
    Write-Verbose -Verbose -Message "Starting MSI install"
    Start-Process "msiexec.exe" -Wait -ArgumentList "/I $msiDesitnation /quiet"
    Write-Verbose -Verbose -Message "Finished MSI install"
}
else {
    throw "Couldn't find installer at $msiDesitnation"
}

wsl --set-default-version 2