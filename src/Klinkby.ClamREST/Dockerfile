FROM mcr.microsoft.com/dotnet/aspnet:5.0.4-alpine3.13 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine3.13 AS build
WORKDIR /src
COPY ["Klinkby.ClamREST.csproj", "."]
RUN dotnet restore "Klinkby.ClamREST.csproj"
COPY . .
RUN dotnet build "Klinkby.ClamREST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Klinkby.ClamREST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
# avoid unnecessary privileges
RUN adduser -D appuser && chown -R appuser /app 
COPY --chown=appuser:appuser --from=publish /app/publish .
USER appuser
ENTRYPOINT ["dotnet", "Klinkby.ClamREST.dll"]
