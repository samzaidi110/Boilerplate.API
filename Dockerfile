# Stage 1: Base image for ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Stage 2: Build image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Clear the default sources.list and other lists in sources.list.d
RUN echo "" > /etc/apt/sources.list
RUN rm -f /etc/apt/sources.list.d/*

# Add Nexus repositories for Debian packages and Debian security updates
RUN mkdir -p /etc/apt/sources.list.d
RUN echo "deb https://nexus.anchoragesource.com:8090/repository/debian-proxy/ stable main" > /etc/apt/sources.list.d/nexus.list
RUN echo "deb https://nexus.anchoragesource.com:8090/repository/debian-security-proxy/ stable-security main" >> /etc/apt/sources.list.d/nexus.list

# Debugging step to verify repository configuration
RUN cat /etc/apt/sources.list.d/nexus.list
RUN cat /etc/apt/sources.list

# Update packages and install necessary tools
RUN apt-get update \
    && apt-get dist-upgrade -y \
    && apt-get install -y --fix-missing openjdk-17-jdk curl unzip \
    && apt-get clean

# Install global .NET tools
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global dotnet-reportgenerator-globaltool
RUN dotnet tool install --global coverlet.console

# Set the PATH to include .NET tools
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy project files
COPY ["./Directory.Packages.props", "./"]
COPY ["libraries", "libraries"]
COPY ["src/Boilerplate.Api/Boilerplate.Api.csproj", "src/Boilerplate.Api/"]
COPY ["src/Boilerplate.Application/Boilerplate.Application.csproj", "src/Boilerplate.Application/"]
COPY ["src/Boilerplate.Domain/Boilerplate.Domain.csproj", "src/Boilerplate.Domain/"]
COPY ["src/Boilerplate.Infrastructure/Boilerplate.Infrastructure.csproj", "src/Boilerplate.Infrastructure/"]
COPY . .

WORKDIR "/src/src/Boilerplate.Api"

# Copy NuGet configuration
COPY nuget.config .

# Restore NuGet packages
RUN dotnet restore

# Stage 3: Publish image for the final runtime
FROM build AS publish

# Run SonarScanner analysis
RUN dotnet sonarscanner begin \
    /k:Solv_Phase_1_boiler-plate_31f6de9c-0df7-4dab-aade-dbd46ccb412b \
    /d:sonar.host.url=https://sonarqube.anchoragesource.com \
    /d:sonar.login=sqa_1a5becc4db199c0db317e75dbd0644b1788e6bea \
    /d:sonar.cs.opencover.reportsPaths=/coverage.opencover.xml \
    /d:sonar.verbose=true 

# Publish the project
RUN dotnet publish "Boilerplate.Api.csproj" -c Release -o /app/publish

# Run tests with coverage
RUN dotnet test \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=opencover \
    /p:CoverletOutput="/coverage"

# End SonarScanner analysis
RUN dotnet sonarscanner end /d:sonar.login=sqa_1a5becc4db199c0db317e75dbd0644b1788e6bea

# Stage 4: Final base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID 
ENTRYPOINT ["dotnet", "Boilerplate.Api.dll"]