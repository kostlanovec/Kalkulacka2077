FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Kalkulacka2077/Kalkulacka2077.csproj", "Kalkulacka2077/"]
RUN dotnet restore "Kalkulacka2077/Kalkulacka2077.csproj"
COPY . .
WORKDIR "/src/Kalkulacka2077"
RUN dotnet build "Kalkulacka2077.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kalkulacka2077.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kalkulacka2077.dll"]
