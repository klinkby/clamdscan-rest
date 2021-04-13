## Summary
REST API with a single endpoint for on-demand malware scanning files using ClamAV.

## Usage
Easiest way to get started is to run the prepackaged [docker-compose](https://github.com/klinkby/clamdscan-rest/blob/main/docker-compose.yml)
```sh
# docker-compose up
```
Or if you prefer to build from source make sure you have .NET 5 SDK and ClamAV installed, then from csproj root run 
```sh
$ dotnet run
```
When up and running just launch the Swagger UI on http://localhost:5000/swagger/index.html to start playing.

## Dependencies
- A running ClamAV daemon to can connect via TCP, typically on port 3310. A docker-compose file is provided to supply this via Docker-internal network.

## Features
- Slim Alpine container image (~65 MB runtime memory)
- Locked-down running with non-root user privileges
- Non-blocking implementation (async/await).
- Performant .net5 Kestrel web server
- Swagger/OpenAPI documentation
- Health endpoint keeps an eye on clamd  TCP connection
- Complete [docker compose](https://github.com/klinkby/clamdscan-rest/blob/main/docker-compose.yml) solution available, binding to [mkodockx/docker-clamav](https://hub.docker.com/repository/docker/mkodockx/docker-clamav) image

## Source code
- [Web API application](https://github.com/klinkby/clamdscan-rest) available as [Docker container](https://hub.docker.com/r/klinkby/clamrest)
- [Clamd proxy library](https://github.com/klinkby/clamdscan) available as [NuGet](https://www.nuget.org/packages/Klinkby.Clam/)

# License
- MIT
