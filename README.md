# GreenMeadows Store

A small full-stack shopping cart (backend API in this repo; React frontend to follow).

See **[SOLUTION.md](./SOLUTION.md)** for the full write-up: architecture, API reference,
how to run locally, assumptions and tradeoffs.

## Quick start

```bash
docker compose up -d                                  # Postgres
dotnet run --project src/GreenMeadows.Store.Api       # API + Swagger (auto-opens)
dotnet test                                           # tests (in-memory Sqlite, no DB needed)
```
