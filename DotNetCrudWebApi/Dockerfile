# Use the official .NET 8 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY DotNetCrudWebApi/*.csproj ./DotNetCrudWebApi/
RUN dotnet restore

# Copy the rest of the source code
COPY DotNetCrudWebApi/. ./DotNetCrudWebApi/
WORKDIR /src/DotNetCrudWebApi

# Build and publish the app
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 8 runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80 (default for ASP.NET Core)
EXPOSE 80

# Set environment variable for ASP.NET Core environment (override at runtime as needed)
# ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "DotNetCrudWebApi.dll"]
