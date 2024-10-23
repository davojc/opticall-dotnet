#!/bin/bash

# Variables
SERVICE_VERSION=0.1
SERVICE_NAME="opticall"
SERVICE_DIR="/usr/local/bin/$SERVICE_NAME"
SERVICE_FILE="$SERVICE_DIR/$SERVICE_NAME"
SERVICE_DESCRIPTION_FILE="/etc/systemd/system/$SERVICE_NAME.service"
SERVICE_BINARY_URL="https://github.com/davojc/opticall-dotnet/releases/download/$SERVICE_VERSION/opticall"
SERVICE_DESCRIPTION_URL="https://github.com/davojc/opticall-dotnet/releases/download/$SERVICE_VERSION/$SERVICE_NAME.service"

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
    echo "Downloading service binary..."
    sudo curl -L "$SERVICE_BINARY_URL" -o "$SERVICE_FILE"
    sudo chmod +x "$SERVICE_FILE" # Make the file executable
}

# Function to download the service description file
download_service_description() {
    echo "Downloading service description file..."
    sudo curl -L "$SERVICE_DESCRIPTION_URL" -o "$SERVICE_DESCRIPTION_FILE"
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
else
    echo "$SERVICE_NAME is not installed. Proceeding with installation..."
    create_directory
    download_service_file
    download_service_description
    install_and_start_service
fi

echo "$SERVICE_NAME installation and startup completed."