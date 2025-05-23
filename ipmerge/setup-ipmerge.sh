#!/bin/bash
set -e

# Check if a parameter is provided
if [ -z "$1" ]; then
  echo "Usage: $0 <release-tag>"
  echo "Example: $0 v1.0.0"
  exit 1
fi

# Variables
DOTNET_INSTALL_DIR="/usr/share/dotnet"
DOTNET_BINARY="$DOTNET_INSTALL_DIR/dotnet"
ALIAS_NAME="ipmerge"
IPMERGE_DIR="/opt/ipmerge"
IPMERGE_DLL="$IPMERGE_DIR/ipmerge.dll"
ALIAS_FILE="/etc/profile.d/${ALIAS_NAME}-alias.sh"
RELEASE_TAG="$1"
RELEASE_ASSET_URL="https://github.com/warrenn/ipmerge/releases/download/${RELEASE_TAG}/linux-x64-net.tar.gz"

# 1. Install .NET Runtime if not present
if ! command -v dotnet >/dev/null 2>&1; then
  echo "Installing .NET $DOTNET_VERSION runtime..."
  wget https://dot.net/v1/dotnet-install.sh -O /tmp/dotnet-install.sh
  chmod +x /tmp/dotnet-install.sh
  /tmp/dotnet-install.sh --install-dir $DOTNET_INSTALL_DIR
  ln -s $DOTNET_BINARY /usr/bin/dotnet
  echo ".NET $DOTNET_VERSION runtime installed successfully."
else
  echo ".NET is already installed."
fi

# 2. Download and extract ipmerge
echo "Downloading ipmerge from GitHub..."
mkdir -p $IPMERGE_DIR
wget -q --show-progress "$RELEASE_ASSET_URL" -O /tmp/linux-x64-net.tar.gz
cd $IPMERGE_DIR
tar -xzf /tmp/linux-x64-net.tar.gz --strip-components=1
chmod +x $IPMERGE_DLL
echo "ipmerge downloaded and extracted to $IPMERGE_DIR."

# 3. Create global alias
echo "Creating global alias '$ALIAS_NAME'..."
echo "alias $ALIAS_NAME='dotnet $IPMERGE_DLL'" > $ALIAS_FILE
chmod +x $ALIAS_FILE
echo "Global alias '$ALIAS_NAME' created."

echo "Installation complete. Please log out and log back in to use the 'ipmerge' command, or run `source $ALIAS_FILE`."
