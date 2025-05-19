FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN apt-get update && apt-get install -y \
    clang lld zlib1g-dev libpthread-stubs0-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY . .

RUN dotnet publish -c Release -r linux-x64 \
    -p:PublishAot=true \
    -p:SelfContained=true \
    -p:IlcUseLld=true \
    -p:IlcUseLLVM=true \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -p:EnableCompressionInSingleFile=true \
    -o /app/out/linux-x64-aot

RUN dotnet publish -c Release -r linux-x64 \
    -p:UseAppHost=true \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    -o /app/out/linux-x64-net

# Final image (optional if you only need the binary)
FROM debian:bookworm-slim
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["./ipmerge"]
