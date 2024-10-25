
# Parameters
$ServiceName = "Opticall"
$RepoPath = "https://github.com/davojc/opticall-dotnet/releases/latest/download"
$FileUrls = @(
    "$RepoPath/$ServiceName.exe",
    "$RepoPath/appsettings.json"
)
$InstallPath = "C:\Program Files\$ServiceName"

# Function to download a file
function Download-File {
    param (
        [string]$url,
        [string]$destinationPath
    )
    Write-Output "Downloading file: $url"
    Invoke-WebRequest -Uri $url -OutFile $destinationPath
}

# Stop the service if it exists
if (Get-Service -Name $ServiceName -ErrorAction SilentlyContinue) {
    Write-Output "Stopping existing service..."
    Stop-Service -Name $ServiceName -Force
}

# Ensure the install directory exists
if (!(Test-Path -Path $InstallPath)) {
    New-Item -ItemType Directory -Path $InstallPath | Out-Null
}

# Download each file to the install path
foreach ($fileUrl in $FileUrls) {
    $fileName = Split-Path -Path $fileUrl -Leaf
    $destinationPath = Join-Path -Path $InstallPath -ChildPath $fileName
    Download-File -url $fileUrl -destinationPath $destinationPath
}

# Register the service if it doesn't exist
if (!(Get-Service -Name $ServiceName -ErrorAction SilentlyContinue)) {
    Write-Output "Installing service..."
    New-Service -Name $ServiceName -BinaryPathName "$InstallPath\$ServiceName.exe" -DisplayName "Your Service Display Name" -StartupType Automatic
}

# Start the service
Write-Output "Starting the service..."
Start-Service -Name $ServiceName

Write-Output "Service installation and update process completed."
