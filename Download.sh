#!/bin/sh

# This script downloads the latest release of the Azure Service Bus Emulator Healthcheck tool 
# and extracts it into the specified output directory.
# Requires: curl, jq, tar

# Usage: Download.sh <OutputDir> <Flags>
# Parameters:
#   <OutputDir> - The directory where the tool will be downloaded and extracted.
#   <Flags> - Optional flags:
#     --retry-max-time <seconds> - Maximum time in seconds for curl retries (default: 300)
#     --retry-delay <seconds> - Delay in seconds between curl retries (default: 30)


set -e

if [ -z "$1" ]; then
  echo "Usage: $0 <OutputDir>"
  exit 1
fi

output_dir="$1"
retry_max_time="300"
retry_delay="30"

shift

while [ "$#" -gt 0 ]; do
  case "$1" in
    --retry-max-time)
      if [ -z "${2:-}" ]; then
        echo "Missing value for --retry-max-time"
        exit 1
      fi
      case "$2" in
        ''|*[!0-9]*)
        echo "Invalid value for --retry-max-time: $2"
        exit 1
          ;;
      esac
      retry_max_time="$2"
      shift 2
      ;;
    --retry-delay)
      if [ -z "${2:-}" ]; then
        echo "Missing value for --retry-delay"
        exit 1
      fi
      case "$2" in
        ''|*[!0-9]*)
        echo "Invalid value for --retry-delay: $2"
        exit 1
          ;;
      esac
      retry_delay="$2"
      shift 2
      ;;
    *)
      echo "Unknown argument: $1"
      echo "Usage: $0 <OutputDir> [--retry-max-time <seconds>] [--retry-delay <seconds>]"
      exit 1
      ;;
  esac
done

# Determine the architecture
arch="$(uname -m)"
if [ "$arch" = "x86_64" ]; then 
  arch="amd64"
elif [ "$arch" = "aarch64" ]; then
  arch="arm64"
else
  echo "Unsupported architecture: $arch"
  exit 1
fi

# Retry options for curl to handle transient network issues
if [ "$retry_delay" -eq 0 ]; then
  echo "--retry-delay must be greater than 0"
  exit 1
fi

# Github has a rate limit of 60 requests per hour for unauthenticated requests. To avoid hitting this limit, we will retry on all errors and set a maximum time for retries.
curl_retry_opts="--retry-all-errors --retry-delay $retry_delay --retry-max-time $retry_max_time --retry 100"

# Echo the curl + jq command for debugging purposes
set -x

# Get the latest release from GitHub
url="$(curl 'https://api.github.com/repos/jeremy-morren/azure-service-bus-emulator-healthcheck/releases/latest' -sSL --fail-with-body $curl_retry_opts \
  | jq -r --arg arch "$arch" '.assets[] | select(.name | endswith($arch + ".tar.gz")) | .browser_download_url')"

# Disable command echoing to avoid printing the URL check in the logs
set +x
if [ -z "$url" ]; then
  echo "Could not find a release for architecture: $arch"
  exit 1
fi

# Echo the curl + tar command for debugging purposes
set -x

# Download and extract the tar artifact
mkdir -p "$output_dir"
curl -fL "$url" $curl_retry_opts | tar -xvz -C "$output_dir"
