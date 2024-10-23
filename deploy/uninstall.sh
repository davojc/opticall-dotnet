#!/bin/bash

# Variables
SERVICE_NAME="opticall"
SERVICE_DIR="/usr/local/bin/$SERVICE_NAME"
SERVICE_DESCRIPTION_FILE="/etc/systemd/system/$SERVICE_NAME.service"


systemctl stop "$SERVICE_NAME.service"
systemctl disable "$SERVICE_NAME.service"
rm "$SERVICE_DESCRIPTION_FILE"
rm -r "$SERVICE_DIR"
systemctl daemon-reload
systemctl reset-failed