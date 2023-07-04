# Starting from MS's dotnet image that has all the SDKs installed,
# build and unit test the app
FROM mcr.microsoft.com/dotnet/sdk:7.0.202-bullseye-slim AS build

COPY . /
WORKDIR /
RUN dotnet restore Apical.TaskManagement.sln

# Build
RUN dotnet build --configuration Release --no-restore Apical.TaskManagement.sln

# Create dotnet artifacts
RUN dotnet publish --no-restore -c Release --output /app Apical.TaskManagement.sln

# Build the deployment container. Switch base images from 'sdk' to
# 'runtime', and use Apline Linux, to reduce image size
FROM mcr.microsoft.com/dotnet/sdk:7.0.202-alpine3.17 AS runtime

# Set up the app to run
WORKDIR /app
COPY --from=build /app .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Apical.TaskManagement.Api.dll"]
