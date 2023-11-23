FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR ./

# Copy the 'csproj' files
COPY TripPlannerBackend.API/*.csproj ./TripPlannerBackend.API/
COPY TripPlannerBackend.DAL/*.csproj ./TripPlannerBackend.DAL/

# Copy the solution file
COPY *.sln ./

# Restore the packages
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR ./
COPY --from=build ./TripPlannerBackend.API/bin/Release/net7.0/publish ./

EXPOSE 80
ENTRYPOINT ["dotnet", "TripPlannerBackend.API.dll"]
