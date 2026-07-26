# Next Session Notes

Date: 2026-07-12

## Current Status

- CI build and image push are working.
- Deploy to VPS now reaches the server and starts the compose stack.
- SSH key auth is fixed.
- `actions/checkout` was added to the deploy job so `scp-action` can see `docker-compose.prod.yml`.
- `environment: production` was added to the deploy job so environment secrets are available.
- Frontend `npm ci` and Nuxt build issues were fixed earlier in this thread.
- Production deploy completed successfully on the VPS.
- MariaDB, backend, and frontend containers are up.
- Role handling was extended in the backend and frontend.
- User responses now include roles, and create/update user flows accept a role.
- The frontend user list shows role badges, and the create/edit modals have role selectors.
- The frontend now loads available roles from `GET /applicationUsers/roles`.
- Common ownership/admin checks were moved into `CurrentUserService` helper methods.

## Production Deployment State

- The production stack now uses `mariadb:10.11-jammy` instead of `mysql:8.0`.
- The VPS CPU could not run the previous MySQL image because of the `x86-64-v2` requirement.
- MariaDB now starts cleanly and is marked healthy.
- The current deploy script writes a `.env` file on the VPS from GitHub secrets.
- The backend successfully ran migrations and seeded the initial admin user.

## What Is Still Missing

- The domain is still pending registration/DNS propagation.
- The frontend Caddy container is trying to obtain TLS certificates for `budget-manager.hu` and `www.budget-manager.hu`, but DNS returns `NXDOMAIN`.
- I could not run a local `.NET` build in this environment because `dotnet` is not installed here.

## Secrets That Need To Exist In GitHub

- `MYSQL_ROOT_PASSWORD`
- `MYSQL_DATABASE`
- `MYSQL_USER`
- `MYSQL_PASSWORD`
- `JWT_SECRET`
- `JWT_VALID_AUDIENCE`
- `JWT_VALID_ISSUER`
- `DEFAULT_ADMIN_EMAIL`
- `DEFAULT_ADMIN_PASSWORD`
- `VPS_HOST`
- `VPS_USER`
- `VPS_SSH_PRIVATE_KEY`
- `GHCR_USERNAME`
- `GHCR_TOKEN`

## Next Steps

1. Continue from the role/ownership refactor if more cleanup is needed in the income/spending handlers.
2. Keep the frontend role selector backed by the backend endpoint instead of hardcoded values.
3. Wait for the domain DNS records to exist.
4. Re-test the public frontend with `curl -I https://budget-manager.hu/` once DNS is live.
5. If needed, inspect Caddy and backend logs on the VPS:
   - `docker compose -f docker-compose.prod.yml --env-file .env logs --tail=100 mysql backend frontend`

## Relevant Files

- [`.github/workflows/ci-cd.yml`](./.github/workflows/ci-cd.yml)
- [`docker-compose.prod.yml`](./docker-compose.prod.yml)
- [`src/Frontend/package.json`](./src/Frontend/package.json)
- [`src/Frontend/package-lock.json`](./src/Frontend/package-lock.json)

## Notes

- The `DRONE_SSH_PREV_COMMAND_EXIT_CODE` warnings are harmless noise from the SSH action.
- `http://127.0.0.1/` on the VPS returns `308` to HTTPS, which is expected.
- `https://127.0.0.1/` currently fails because Caddy does not have a valid certificate yet.
- The backend is healthy and listening on port 80 inside the container.
- The key remaining blocker is DNS/TLS readiness, not application code or DB initialization.
- For the codebase, the next good cleanup is to keep pushing ownership logic into the shared helper instead of reintroducing per-handler checks.

## Reminder

Closed on 2026-07-16.

- Frontend dev now starts cleanly after the Nuxt/i18n fixes.
- Resume with deploy verification, then DNS/TLS checks on the live domain.
- If anything regresses, recheck `src/Frontend/nuxt.config.ts` and `src/Frontend/i18n.config.mjs` first.
