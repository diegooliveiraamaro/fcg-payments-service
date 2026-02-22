# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet publish Payments.Api.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

EXPOSE 80

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Payments.Api.dll"]
