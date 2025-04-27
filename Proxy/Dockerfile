ARG DOTNET_VERSION=9.0
ENV CONTAINER=1

# For CORS configuration
# This needs to be set up in docker compose  file
ENV ALLOWED_CORS_ORIGIN ""

# First we need to build the application
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release
# Create and go into src folder 
WORKDIR /src

# Copy Proxy project, and Shared since it's Proxy dependence
# Path relative from .sln thus we need to specify [Project]/[Proxy].csproj 

COPY ["Proxy/Proxy.csproj", "Proxy/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore NuGet and copy artifacts to /src
RUN dotnet restore "Proxy/Proxy.csproj" 
COPY . .

# Create and go into src/Proxy
WORKDIR "/src/Proxy"

# Build the app and output files into /app/build
RUN dotnet build "./Proxy.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app and copy files to /app/publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Proxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Copy and run the app
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final

# Create and go into app folder
WORKDIR /app

# Copy files from build stage, (i guess --from is needed bcs it was in different image but idk) 
# that dotnet publish created, into /app directory
COPY --from=publish /app/publish .

# And finally run the app
ENTRYPOINT ["dotnet", "Proxy.dll"]

EXPOSE 8080
EXPOSE 8081