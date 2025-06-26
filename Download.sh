#!/bin/sh

# This script downloads the latest release of the Azure Service Bus Emulator Healthcheck tool
# and extracts it into the specified output directory.
# Usage: Download.sh <OutputDir>
# Requires: curl, jq, tar

set -ex

if [ -z "$1" ]; then
    echo "Usage: $0 <OutputDir>"
    exit 1
fi

# Determine the architecture
arch="$(uname -m)"
if [ "$arch" = "x86_64" ]; then 
    arch="amd64"
elif [ "$$arch" = "aarch64" ]; then
    arch="arm64"
else
    echo "Unsupported architecture: $arch"
    exit 1
fi

# Get the latest release from GitHub
url="$(curl 'https://api.github.com/repos/jeremy-morren/azure-service-bus-emulator-healthcheck/releases/latest' -sSf \
    | jq -r --arg arch "$arch" '.assets[] | select(.name | endswith($arch + ".tar.gz")) | .browser_download_url')"

# Download and extract the tar artifact
mkdir -p "$1"
curl -L "$url" | tar -xvz -C "$1"