# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

# Copy the 'csproj' and 'sln' files
COPY *.sln ./
COPY TripPlannerBackend.API/*.csproj ./TripPlannerBackend.API/
COPY TripPlannerBackend.DAL/*.csproj ./TripPlannerBackend.DAL/

# Restore the packages
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./


ENTRYPOINT ["dotnet", "TripPlannerBackend.API.dll"]
