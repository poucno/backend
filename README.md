# Poucno Backend

REST API for Poucno, educational games for kids. Teachers log in and create
question collections; kids play without an account.

Built with ASP.NET Core (.NET 10), MediatR (CQRS), AutoMapper, EF Core + SQLite
and ASP.NET Identity.

## Run

```sh
dotnet run --project src/Api      # http://localhost:8080, Swagger UI at /swagger
docker compose up --build         # same, with the database in a volume
```

The database is created and migrated automatically on startup.

## Structure

Each feature is a slice under `src/Api/Features/<Name>/` (commands, queries,
DTOs, mappings). HTTP routes live in `src/Api/Endpoints/`, entities and
migrations in `src/Api/Data/`.

New migration: `dotnet ef migrations add <Name> --project src/Api`

## API

| Route | Login | Description |
|---|---|---|
| `/auth/register`, `/auth/login` | no | ASP.NET Identity, returns a bearer token |
| `GET /games` | no | Games from the `Games` section in `appsettings.json` |
| `GET /collections/{id}` | no | One collection with questions (the id is the share link) |
| `GET /collections` | yes | The teacher's own collections |
| `POST`, `PUT`, `DELETE /collections` | yes | Manage own collections |

A question is `{ question, hint, possibleAnswers, answer }`. Each game sets how
many `possibleAnswers` it takes (`MinAnswers`/`MaxAnswers`); word guessing takes
none, only `answer`.

## Adding a game

Add an entry to `Games:Items` in `appsettings.json` (id, title, url,
thumbnailUrl, min/max answers). No code change needed.
