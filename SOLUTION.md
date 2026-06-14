# GreenMeadows Store — Solution

A small full-stack shopping cart. This document covers the **backend API** (the focus so far);
the React frontend is the next stage of work.

## Stack & key choices

| Concern | Choice | Why |
|---|---|---|
| API | ASP.NET Core 8 Web API (controllers) | Controllers give clear, conventional resource grouping that's easy to navigate and review. |
| Persistence | EF Core 8 + PostgreSQL (Npgsql) | EF gives a clean mapping, migrations, and seed data; Postgres is production-realistic. |
| Versioning | `Asp.Versioning` → `/api/v1/...` | URL-segment versioning is explicit and friendly to consumers. |
| Errors | RFC 7807 `ProblemDetails` | One consistent error shape across validation, 404, 409, and 500. |
| Money | `decimal` (`numeric(18,2)`) | Never use floating point for currency; totals are computed once, server-side. |
| Tests | xUnit + Sqlite (in-memory) + `WebApplicationFactory` | Unit-test the cart rules; integration-test the real HTTP pipeline with no external DB. |

## Architecture

A single API project with concerns split by folder — deliberate separation without the ceremony
of four projects for an exercise this size:

```
src/GreenMeadows.Store.Api
  Domain/          Entities (Product, Cart, CartItem)
  Data/            DbContext, seed data, EF migrations
  Dtos/            Request/response contracts (never expose entities directly)
  Services/        Cart & product business logic (pricing, stock rules) — the unit-tested core
  Controllers/     Thin HTTP layer: validate, call a service, map result
  Infrastructure/  Cross-cutting: exception→ProblemDetails handler, startup migration
tests/GreenMeadows.Store.Tests
```

Controllers stay thin: all cart mutation logic lives in `CartService`, so the rules have one home
that's easy to test in isolation. Domain exceptions (`NotFoundException`, `BusinessRuleException`)
are translated centrally to ProblemDetails by `GlobalExceptionHandler`, so controllers never
write status-code plumbing.

## API

Base path `/api/v1`.

| Method | Route | Purpose | Codes |
|---|---|---|---|
| GET | `/products` | List catalogue | 200 |
| GET | `/products/{id}` | Single product | 200, 404 |
| POST | `/carts` | Create empty cart | 201 (+`Location`) |
| GET | `/carts/{id}` | Cart + money breakdown | 200, 404 |
| POST | `/carts/{id}/items` | Add product / increase qty | 200, 400, 404, 409 |
| PATCH | `/carts/{id}/items/{productId}` | Set absolute qty | 200, 400, 404, 409 |
| DELETE | `/carts/{id}/items/{productId}` | Remove line | 200, 404 |

Every cart response carries the server-computed money breakdown:

```json
{
  "id": "8cda889a-...",
  "items": [
    { "productId": "1111...", "name": "Garden Spade", "imageUrl": "...",
      "unitPrice": 24.99, "quantity": 2, "lineTotal": 49.98 }
  ],
  "subtotal": 49.98, "taxRate": 0.20, "tax": 10.00, "total": 59.98, "currency": "NZD"
}
```

Errors are ProblemDetails, e.g. adding beyond stock:

```json
{ "title": "Request conflicts with current state", "status": 409,
  "detail": "Only 3 of 'Pruning Shears' in stock; requested 9." }
```

## Running locally

Prerequisites: .NET 8 SDK, Docker.

```bash
# 1. Start Postgres (mapped to host port 5432 to avoid clashing with a local 5432)
docker compose up -d

# 2. Run the API (migrations + seed apply automatically on startup)
dotnet run --project src/GreenMeadows.Store.Api
```

### Swagger / OpenAPI

The default `http` profile sets `ASPNETCORE_ENVIRONMENT=Development` and auto-opens your browser
at Swagger UI when the API starts:

```
http://localhost:5229/swagger
```

(Run the `https` profile with `dotnet run --launch-profile https` → `https://localhost:7177/swagger`.)

Swagger is only mapped in the Development environment. If you launch with a custom URL, the path is
always `/swagger` on whatever host:port you bind, e.g. `http://localhost:5080/swagger`. The raw
OpenAPI document is at `/swagger/v1/swagger.json`.

```bash
# Tests (no database required — they use in-memory Sqlite)
dotnet test
```

Config lives in `appsettings.json`

## Cart identity & "survives a refresh"

The cart is **server-side** (the requirement rules out a client-only cart). `POST /carts` returns an
opaque GUID; the frontend persists that id in `localStorage` and re-fetches the cart on load. So the
cart state itself lives on the server, and only the *pointer* to it survives the refresh.

## Assumptions & tradeoffs

- **No auth.** Carts are anonymous, identified by an unguessable GUID. Guest→user merge is a stretch goal.
- **Stock enforcement included** (cheap, and shows real validation) but stock is **not decremented** —
  there's no checkout/reservation step, so a cart only checks availability, it doesn't hold inventory.
- **Migrate-on-startup** is convenient for the exercise; a production deploy would run migrations as a
  separate gated step.
- **Single project, folders not layers** — pragmatic for the scope; a larger system would split
  Domain/Application/Infrastructure into projects to enforce dependency direction.
- **Tax** is a flat configurable rate on the subtotal — no per-region or per-product tax rules.
