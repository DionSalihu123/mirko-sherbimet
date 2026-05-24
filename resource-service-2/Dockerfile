FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5295

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["resource-service-2.csproj", "./"]
RUN dotnet restore "resource-service-2.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "resource-service-2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "resource-service-2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "resource-service-2.dll"]
