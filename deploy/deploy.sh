#!/bin/bash

# If passed, set environment variables
envTarget=""
envGroup=""

# Variables
SERVICE_NAME="opticall"

arch=$(uname -m)

srcFile="$SERVICE_NAME"

if [[ "$arch" == "aarch64" ]]; then
    srcFile="opticall-arm"
elif [[ "$arch" == "armv7l" || "$arch" == "armv61" ]]; then
    srcFile="opticall-arm-32"
fi

SERVICE_DIR="/usr/local/bin/$SERVICE_NAME"
SERVICE_FILE="$SERVICE_DIR/$SERVICE_NAME"
SERVICE_CONFIG_FILE="$SERVICE_DIR/appsettings.json"
SERVICE_DESCRIPTION_FILE="/etc/systemd/system/$SERVICE_NAME.service"
SERVICE_DOWNLOAD_URL="https://github.com/davojc/opticall-dotnet/releases/latest/download"
SERVICE_BINARY_URL="$SERVICE_DOWNLOAD_URL/$srcFile"
SERVICE_CONFIG_URL="$SERVICE_DOWNLOAD_URL/settings.yml"
SERVICE_DESCRIPTION_URL="$SERVICE_DOWNLOAD_URL/$SERVICE_NAME.service"

# Function to check if the service exists
check_service_exists() {
    systemctl list-unit-files --type=service | grep -q "$SERVICE_NAME.service"
}

# Function to stop the service if it is running
stop_service() {
    echo "Checking if $SERVICE_NAME is running..."
    if systemctl is-active --quiet "$SERVICE_NAME"; then
        echo "$SERVICE_NAME is running. Stopping it..."
        sudo systemctl stop "$SERVICE_NAME"
    else
        echo "$SERVICE_NAME is not running."
    fi
}

# Function to create the directory for the service files if it doesn't exist
create_directory() {
    if [ ! -d "$SERVICE_DIR" ]; then
        echo "Creating directory $SERVICE_DIR..."
        sudo mkdir -p "$SERVICE_DIR"
    else
        echo "Directory $SERVICE_DIR already exists."
    fi
}

# Function to download the service binary
download_service_file() {
    echo "Downloading service binary from $SERVICE_BINARY_URL..."
    sudo curl -L "$SERVICE_BINARY_URL" -o "$SERVICE_FILE"
    sudo chmod +x "$SERVICE_FILE" # Make the file executable
}

# Function to download the service description file
download_service_description_and_settings() {
    echo "Downloading default config file..."
    sudo curl -L "$SERVICE_CONFIG_URL" -o "$SERVICE_CONFIG_FILE"
    echo "Downloading service description file..."
    sudo curl -L "$SERVICE_DESCRIPTION_URL" -o "$SERVICE_DESCRIPTION_FILE"
}

update_settings_with_args() {
    while getopts "tg" opt; do
    case $opt in
        t)
            envTarget="$OPTARG"
            ;;
        g)
            envGroup="$OPTARG"
            ;;
        esac
    done

    if [ "$envTarget" -neq "" ]; then
        sed -i 's|target: .*|target: $envTarget|' $SERVICE_CONFIG_URL
    fi

    if [ "$envGroup" -neq "" ]; then
        sed -i 's|group: .*|group: $envGroup|' $SERVICE_CONFIG_URL
    fi
}

# Function to enable and start the service
install_and_start_service() {
    echo "Reloading systemd..."
    sudo systemctl daemon-reload

    echo "Enabling $SERVICE_NAME service..."
    sudo systemctl enable "$SERVICE_NAME"

    echo "Starting $SERVICE_NAME service..."
    sudo systemctl start "$SERVICE_NAME"
}

# Function to start the service
start_service() {
    echo "Starting $SERVICE_NAME service..."
    sudo systemctl start "$SERVICE_NAME"
}

# Main script logic
echo "Starting the installation process for $SERVICE_NAME..."

if check_service_exists; then
    echo "$SERVICE_NAME is already installed."
    stop_service
    download_service_file
    update_settings_with_args
    start_service
else
    echo "$SERVICE_NAME is not installed. Proceeding with installation..."
    create_directory
    download_service_file
    download_service_description_and_settings
    update_settings_with_args
    install_and_start_service
fi

echo "$SERVICE_NAME installation and startup completed."