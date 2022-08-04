FROM mcr.microsoft.com/dotnet/aspnet:6.0.7-alpine3.16 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.16 AS build
WORKDIR /src
COPY ["Klinkby.ClamREST.csproj", "."]
RUN dotnet restore "Klinkby.ClamREST.csproj"
COPY . .
RUN dotnet build "Klinkby.ClamREST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Klinkby.ClamREST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# drop root privileges
RUN adduser -D appuser && chown -R appuser /app 
COPY --chown=appuser:appuser --from=publish /app/publish .
USER appuser

# use exposed port
ENV ASPNETCORE_URLS="http://+:5000"
# don't phone home to MS
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV COMPLUS_ENABLEDIAGNOSTICS=0

ENTRYPOINT ["dotnet", "Klinkby.ClamREST.dll"]
