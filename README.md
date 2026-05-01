# Enterprise SaaS Backend Platform (ASP.NET Core)

Production-style backend platform showcasing:

- Clean Architecture (Domain, Application, Infrastructure, API)
- Tenant-aware design with scoped context resolution
- JWT authn/authz + rotating refresh tokens with revocation
- EF Core + PostgreSQL persistence with startup migration execution
- Structured logging (Serilog) and OpenTelemetry traces/metrics
- Health endpoints (`/health/live`, `/health/ready`), rate limiting, HSTS
- Docker and Kubernetes deployment assets

## Quickstart (local)

```bash
dotnet restore SaaSPlatform.sln
docker compose up -d db
dotnet run --project src/SaaSPlatform.Api/SaaSPlatform.Api.csproj
```

Seeded credentials:

- Tenant: `acme`
- Email: `admin@acme.io`
- Password: `P@ssword123!`

## Auth flow

1. Call `POST /api/v1/auth/login` to receive access + refresh tokens.
2. Use access token for protected APIs.
3. Call `POST /api/v1/auth/refresh` with refresh token to rotate tokens.
4. Call `POST /api/v1/auth/revoke` to revoke refresh token on logout.

## Core endpoints

- `POST /api/v1/auth/login`
- `POST /api/v1/auth/refresh`
- `POST /api/v1/auth/revoke`
- `GET /api/v1/projects` (requires JWT)
- `POST /api/v1/projects` (requires JWT)
- `GET /api/v1/tenants` (requires `TenantAdmin` role)
- `POST /api/v1/tenants` (requires `TenantAdmin` role)

## Operations

- `docker-compose.yml` runs API + PostgreSQL + OpenTelemetry Collector.
- `ops/k8s` contains baseline deployment and service manifests.
- `ops/otel-collector.yaml` defines OTLP ingest and debug export pipeline.
