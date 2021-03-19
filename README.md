## Summary
REST API with a single endpoint for on-demand malware scanning files using ClamAV.

## Dependencies
- A running ClamAV daemon it can connect via TCP, typically on port 3310. A docker-compose file is provided to supply this via Docker-internal network.

## Features
- Slim Alpine container image (~65 MB runtime memory)
- Locked-down running with non-root user privileges
- Non-blocking implementation (async/await).
- Performant .net5 Kestrel web server
- Swagger/OpenAPI documentation
- Health endpoint keeps an eye on clamd  TCP connection
- Complete [docker compose](https://github.com/klinkby/clamdscan-rest/blob/main/docker-compose.yml) solution available, binding to [mkodockx/docker-clamav](https://hub.docker.com/repository/docker/mkodockx/docker-clamav) image

## Source code
- [Web API application](https://github.com/klinkby/clamdscan-rest) available as [Docker container](https://hub.docker.com/repository/docker/klinkby/clamrest)
- [Clamd proxy library](https://github.com/klinkby/clamdscan) available as [NuGet](https://www.nuget.org/packages/Klinkby.Clam/)

# License
- MIT
