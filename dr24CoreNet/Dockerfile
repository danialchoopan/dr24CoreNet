# Multi-stage Dockerfile for the whole solution
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all projects
COPY ["dr24CoreNet.slnx", "./"]
COPY ["dr24CoreNet.Domain/dr24CoreNet.Domain.csproj", "dr24CoreNet.Domain/"]
COPY ["dr24CoreNet.Application/dr24CoreNet.Application.csproj", "dr24CoreNet.Application/"]
COPY ["dr24CoreNet.Infrastructure/dr24CoreNet.Infrastructure.csproj", "dr24CoreNet.Infrastructure/"]
COPY ["dr24CoreNet.WebAPI/dr24CoreNet.WebAPI.csproj", "dr24CoreNet.WebAPI/"]
COPY ["dr24CoreNet.WebUI/dr24CoreNet.WebUI.csproj", "dr24CoreNet.WebUI/"]

RUN dotnet restore

COPY . .

# Build API
RUN dotnet publish "dr24CoreNet.WebAPI/dr24CoreNet.WebAPI.csproj" -c Release -o /app/api

# Build UI
RUN dotnet publish "dr24CoreNet.WebUI/dr24CoreNet.WebUI.csproj" -c Release -o /app/ui

# Final stage for API
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS api-final
WORKDIR /app
COPY --from=build /app/api .
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]

# Final stage for UI
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS ui-final
WORKDIR /app
COPY --from=build /app/ui .
ENTRYPOINT ["dotnet", "dr24CoreNet.WebUI.dll"]
