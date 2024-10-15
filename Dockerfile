# Stage 1: Build the application
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Copy all source files
COPY . /source

# Set the working directory
WORKDIR /source/RHCQS_BE

# Argument for target architecture
ARG TARGETARCH

# Restore and publish the application
RUN --mount=type=cache,id=nuget_cache,target=/root/.nuget/packages \
    dotnet restore \
    && dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Stage 2: Final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Install necessary dependencies for wkhtmltox and PDF generation
RUN apk add --no-cache \
    libxrender \
    libxext \
    libjpeg-turbo \
    libpng \
    libgcc \
    musl \
    fontconfig \
    ttf-dejavu \
    icu-libs \
    gcompat \
    zlib-dev

# Copy your offline wkhtmltox binary from your local source to the container
COPY ./ExternalLibraries/libwkhtmltox.so /usr/lib/libwkhtmltox.so

# Ensure the binaries have the correct permissions
RUN chmod +x /usr/lib/libwkhtmltox.so

# Verify the dependencies of the .so library
RUN ldd /usr/lib/libwkhtmltox.so

# Set environment variable for globalization support
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Set the working directory for the app
WORKDIR /app

# Copy the app from the build stage
COPY --from=build /app .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "RHCQS_BE.dll"]
