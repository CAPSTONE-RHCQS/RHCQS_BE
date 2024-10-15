# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Copy all files to the source directory
COPY . /source

# Set working directory
WORKDIR /source/RHCQS_BE

ARG TARGETARCH

# Build the application
RUN --mount=type=cache,id=nuget_cache,target=/root/.nuget/packages \
    dotnet restore \
    && dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Create a new stage for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final


# Install dependencies including ICU
RUN apk add --no-cache \
    libxrender \
    libxext \
    libjpeg-turbo \
    libpng \
    libgcc \
    musl \
    fontconfig \
    ttf-dejavu \
    icu-libs

# Copy libwkhtmltox.so from the external libraries folder
COPY ./ExternalLibraries/libwkhtmltox.so /usr/lib/libwkhtmltox.so

# Set environment variable for globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Set working directory for the app
WORKDIR /app

# Copy everything needed to run the app from the "build" stage
COPY --from=build /app ./

# Uncomment if you need to run as a non-privileged user
# USER $APP_UID

ENTRYPOINT ["dotnet", "RHCQS_BE.dll"]
