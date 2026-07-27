#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")/.."

docker compose -f docker-compose.prod.yml --env-file .env pull
docker compose -f docker-compose.prod.yml --env-file .env up -d --remove-orphans
