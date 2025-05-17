#!/bin/bash
set -e

# Build image
docker build -t ipmerge-builder .

# Create output folder
mkdir -p output

# Copy binary from container to host
docker create --name temp ipmerge-builder
docker cp temp:/app/ipmerge ./output/ipmerge
docker rm temp

echo "Native AOT binary saved to ./output/ipmerge"
