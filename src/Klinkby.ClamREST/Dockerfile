FROM mcr.microsoft.com/dotnet/aspnet:5.0.4-alpine3.13 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13 AS build
WORKDIR /src
COPY ["src/Klinkby.ClamREST/Klinkby.ClamREST.csproj", "src/Klinkby.ClamREST/"]
RUN dotnet restore "src/Klinkby.ClamREST/Klinkby.ClamREST.csproj"
COPY . .
WORKDIR "/src/src/Klinkby.ClamREST"
RUN dotnet build "Klinkby.ClamREST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Klinkby.ClamREST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Klinkby.ClamREST.dll"]
