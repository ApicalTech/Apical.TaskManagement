FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy everything else and build
COPY . ./src
WORKDIR /app/src/Apical.TaskManagement.Api
RUN dotnet publish -c Release -o out --configfile docker.nuget.config

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app/src/Apical.TaskManagement.Api
COPY --from=build-env /app/src/Apical.TaskManagement.Api/out/ .
ENTRYPOINT ["dotnet", "Apical.TaskManagement.Api.dll"]