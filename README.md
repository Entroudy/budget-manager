# Budget Manager

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

### 2. Configure the backend

The backend uses a connection string and JWT-related environment variables.
An example file is available at:

- `src/Backend/BudgetManager/BudgetManager.Presentation/example.env`

Typical local values from the repository:

- `DB_CONNECTION=server=localhost;port=13006;user=admin;password=admin;database=bm_db`
- `DEFAULT_ADMIN_EMAIL=admin@entroudy.com`
- `DEFAULT_ADMIN_PASSWORD=Almafa123!`

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

## Default Admin Account

The repository includes a default admin account for local development:

- Email: `admin@entroudy.com`
- Password: `Almafa123!`

If you change these values, update your local environment accordingly.

## Notes

- The backend solution is under `src/Backend/BudgetManager`
- The frontend uses Nuxt and Pinia, so most UI state lives in `src/Frontend/stores`
- Swagger is available in the backend for API exploration during development
- The backend targets `.NET 10`, but the EF Core and Identity packages are kept on `9.x` because the current Pomelo MySQL provider is built against EF Core 9
