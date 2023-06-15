#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-jammy AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy AS build
WORKDIR /src
COPY ["src/MassTransitClient.API/MassTransitClient.API.csproj", "MassTransitClient.API/"]
COPY ["src/MassTransitClient.Infrastructure/MassTransitClient.Infrastructure.csproj", "MassTransitClient.Infrastructure/"]
COPY ["src/MassTransitClient.Consumers/MassTransitClient.Consumers.csproj", "MassTransitClient.Consumers/"]
COPY ["src/MassTransitClient.Domain/MassTransitClient.Domain.csproj", "MassTransitClient.Domain/"]
RUN dotnet restore "MassTransitClient.API/MassTransitClient.API.csproj"
COPY . .
WORKDIR "src/MassTransitClient.API"
RUN dotnet build "MassTransitClient.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MassTransitClient.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MassTransitClient.API.dll"]