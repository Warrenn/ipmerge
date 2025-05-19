#!/bin/bash
set +e

cd "$(dirname "$0")" || exit 1
# Build image
docker system prune -af
docker build --no-cache --platform=linux/amd64 -t ipmerge-builder .

# Create output folder
mkdir -p output

# Copy binary from container to host
docker create --name temp ipmerge-builder
docker cp temp:/app ./output
docker rm temp
docker system prune -af

echo "Native AOT binary saved to ./output/app/linux-x64-aot/ipmerge"
echo ".Net binary saved to ./output/app/linux-x64-net/ipmerge"
echo "Build complete."
