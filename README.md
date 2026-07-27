# Budget Manager

[![CI/CD](https://github.com/Entroudy/budget-manager/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/Entroudy/budget-manager/actions/workflows/ci-cd.yml)

Budget Manager is a full-stack personal finance application for tracking incomes, spendings, and users. The repository contains:

- A `.NET 10` backend API
- A `Nuxt 3` frontend
- A MySQL database used by the backend

## Features

- Authentication with JWT
- User management
- Income management
- Spending management
- Pagination, filtering, search, and sorting support in the backend
- Docker-based local development setup

## Tech Stack

- Backend: ASP.NET Core 10, Entity Framework Core, MediatR, FluentValidation, Swagger
- Frontend: Nuxt 3, Vue 3, Pinia, Tailwind CSS, TypeScript
- Database: MySQL 8

## Project Structure

- `src/Backend/BudgetManager` - backend solution and API
- `src/Frontend` - frontend application
- `docker-compose.yml` - local MySQL and app container setup

## Requirements

- `.NET 10 SDK`
- `Node.js 20.x`
- `npm`
- `MySQL 8` if you run the backend without Docker

## Local Setup

### 1. Start the database

If you want the easiest local setup, start MySQL with Docker:

```bash
docker compose up -d mysql
```

The local Docker setup reads its values from a root `.env` file. A template is available at:

- `.env.example`

Copy it to `.env` and adjust the passwords if you want to override the defaults.

### 2. Configure the backend

The backend uses a connection string and JWT-related environment variables.
An example file is available at:

- `src/Backend/BudgetManager/BudgetManager.Presentation/example.env`

Typical local values:

- `DB_CONNECTION=server=localhost;port=13006;user=admin;password=bm-db-password;database=bm_db`
- `DEFAULT_ADMIN_EMAIL=change-me-admin-email`
- `DEFAULT_ADMIN_PASSWORD=change-me-admin-password`

### 3. Run the backend

From the backend presentation project:

```bash
cd src/Backend/BudgetManager/BudgetManager.Presentation
dotnet restore
dotnet run
```

The API is configured for local development on port `4002` in the example environment file.

### 4. Run the frontend

```bash
cd src/Frontend
npm install
npm run dev
```

The frontend scripts are:

- `npm run dev`
- `npm run build`
- `npm run generate`
- `npm run preview`

## Docker Setup

The repository also includes `docker-compose.yml`, which starts:

- `mysql` on port `13006`
- `budget-manager` on port `4000`
- `phpmyadmin` on port `19107`

If you use the Docker compose setup, make sure the backend container environment variables match your build and deployment setup.
The root `docker-compose.yml` now uses `.env`-driven defaults for the database and JWT settings, so local secrets no longer need to be hardcoded into the compose file.

## Production Deployment

The repository also includes a VPS-oriented production stack:

- `docker-compose.prod.yml`
- `src/Frontend/Dockerfile`
- `src/Frontend/Caddyfile`
- `.github/workflows/ci-cd.yml`

The intended flow is:

1. push to `main`
2. GitHub Actions validates the backend and frontend
3. Docker images are built and pushed to GHCR
4. the VPS pulls the new images
5. `docker compose up -d` replaces the running containers

The production stack uses:

- `frontend` as the public entrypoint on ports `80` and `443`
- `backend` on the internal Docker network only
- `mysql` as the database container

The frontend container terminates TLS with Caddy and proxies `/api/*` to the backend container, so the app can use the same origin in production.

Before the first deploy, create a `.env` file next to `docker-compose.prod.yml` from `.env.production.example` and fill in:

- MySQL credentials
- JWT secret
- JWT issuer and audience
- default admin credentials

The deploy workflow expects these GitHub repository secrets:

- `VPS_HOST`
- `VPS_USER`
- `VPS_SSH_PRIVATE_KEY`
- `GHCR_USERNAME`
- `GHCR_TOKEN`
- `MYSQL_ROOT_PASSWORD`
- `MYSQL_DATABASE`
- `MYSQL_USER`
- `MYSQL_PASSWORD`
- `JWT_SECRET`
- `JWT_VALID_AUDIENCE`
- `JWT_VALID_ISSUER`
- `DEFAULT_ADMIN_EMAIL`
- `DEFAULT_ADMIN_PASSWORD`
- `NUXT_AUTH0_DOMAIN`
- `NUXT_AUTH0_CLIENT_ID`
- `NUXT_AUTH0_AUDIENCE`

The VPS does not need a full repository checkout. The workflow creates `/opt/budget-manager`, uploads `docker-compose.prod.yml`, writes `.env`, logs in to GHCR, and then runs `docker compose pull` and `docker compose up -d`.

## Default Admin Account

The repository does not include a committed default admin credential pair. Set `DEFAULT_ADMIN_EMAIL` and `DEFAULT_ADMIN_PASSWORD` in your local environment before starting the backend.

## Notes

- The backend solution is under `src/Backend/BudgetManager`
- The frontend uses Nuxt and Pinia, so most UI state lives in `src/Frontend/stores`
- Swagger is available in the backend for API exploration during development
- The backend targets `.NET 10`, but the EF Core and Identity packages are kept on `9.x` because the current Pomelo MySQL provider is built against EF Core 9

## Development Notes

- I used AI assistance while implementing parts of the frontend and while validating some architectural decisions. The final implementation, review, and integration choices were still made and owned by me.
