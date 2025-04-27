ARG DOTNET_VERSION=9.0
ENV CONTAINER=1

# This needs to be set up in docker compose file
ENV PROXY_URL ""
ENV PROXY_PORT ""

# Set up NginX
FROM nginx AS base
EXPOSE 80
EXPOSE 443

# First build the app
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
ARG BUILD_CONFIGURATION=Release

# Copy projects to /src folder 
# Path is relative from .sln so also specify the folder
WORKDIR /src
COPY ["Schedule/Schedule.csproj", "Schedule/"]
COPY ["Schedule.Application/Schedule.Application.csproj", "Schedule.Application/"]
COPY ["Schedule.Domain/Schedule.Domain.csproj", "Schedule.Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore NuGet and copy output to /src
RUN dotnet restore "Schedule/Schedule.csproj"
COPY . .

# Create /src/Schedule and built the project
WORKDIR "/src/Schedule"
RUN dotnet build "./Schedule.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Schedule.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY Schedule/nginx.conf /etc/nginx/nginx.conf