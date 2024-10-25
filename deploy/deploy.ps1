# Setting env variables if provided
param (
    [Parameter(Mandatory=$false)]
    [Alias("t")]
    [string]$Target,

    [Parameter(Mandatory=$false)]
    [Alias("g")]
    [string]$Group
)

# Parameters
$ServiceName = "Opticall"
$RepoPath = "https://github.com/davojc/opticall-dotnet/releases/latest/download"
$FileUrls = @(
    "$RepoPath/$ServiceName.exe",
    "$RepoPath/settings.yml",
    "$RepoPath/logging.json"
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

$yamlFilePath = "$InstallPath\settings.yml"
$yamlContent = Get-Content -Path $yamlFilePath -Raw

if ($Target) {
    $yamlContent = $yamlContent -replace 'target:[a-zA-Z ]+', "target: $Target"
}

if ($Group) {
    $yamlContent = $yamlContent -replace 'group:[a-zA-Z ]+', "group: $Group"
}


# Write it to the file
Set-Content -Path $yamlFilePath -Value $yamlContent

# Register the service if it doesn't exist
if (!(Get-Service -Name $ServiceName -ErrorAction SilentlyContinue)) {
    Write-Output "Installing service..."
    New-Service -Name $ServiceName -BinaryPathName "$InstallPath\$ServiceName.exe" -DisplayName "$ServiceName" -StartupType Automatic
}

# Start the service
Write-Output "Starting the service..."
Start-Service -Name $ServiceName

Write-Output "Service installation and update process completed."
