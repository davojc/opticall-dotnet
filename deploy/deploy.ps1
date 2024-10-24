# Variables
$serviceName = "Opticall"
$githubRepo = "https://github.com/davojc/opticall-dotnet/releases/latest/download"
$downloadExe = "$githubRepo/opticall.exe"
$downloadConfig = "$githubRepo/appsettings.json"
$downloadPath = "C:\Program Files\$serviceName"
$destExe = "$downloadPath\opticall.exe"
$destConfig = "$downloadPath\appsettings.json"
$nssmPath = "C:\nssm\nssm.exe"  # Path to NSSM executable

# Check if the service exists
$service = Get-Service -Name $serviceName -ErrorAction SilentlyContinue

if ($service) {
    # If the service exists, stop it
    if ($service.Status -eq 'Running') {
        Write-Output "Stopping the service '$serviceName'..."
        Stop-Service -Name $serviceName -Force
    }
}

# Create the directory if it doesn't exist
if (-not (Test-Path $downloadPath)) {
    New-Item -Path $downloadPath -ItemType Directory -Force
}

# Download exe
try {
    Write-Output "Download $downloadExe"
    Invoke-WebRequest -Uri $downloadExe -OutFile $destExe -UseBasicParsing
    Write-Output "$destExe download completed."
} catch {
    Get-Error
    #Write-Error "Failed to download $destExe: $_"
    exit 1
}

# Download app settings
try {
    Invoke-WebRequest -Uri $asset.browser_download_url -OutFile $destConfig -UseBasicParsing
    Write-Output "$destConfig download completed."
} catch {
    Get-Error
    #Write-Error "Failed to download $destConfig: $_"
    exit 1
}

# Install the service using NSSM if it doesn't exist
if (-not $service) {
    Write-Output "Installing the service '$serviceName' using NSSM..."
    $nssmInstallCommand = "$nssmPath install $serviceName `"$destExe`""
    try {
        Invoke-Expression $nssmInstallCommand
        Write-Output "Service '$serviceName' installed successfully."
    } catch {
        Write-Error "Failed to install the service using NSSM: $_"
        exit 1
    }
}

# Start the service if it was previously stopped or newly installed
Write-Output "Starting the service '$serviceName'..."
try {
    Start-Service -Name $serviceName
    Write-Output "Service 'Opticall' started successfully."
} catch {
    Write-Error "Failed to start the service: $_"
    exit 1
}

Write-Output "Operation completed."
