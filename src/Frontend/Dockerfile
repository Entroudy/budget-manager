FROM node:20-alpine AS build

WORKDIR /app

COPY src/Frontend/package*.json ./
RUN npm ci

COPY src/Frontend/ ./

ARG NUXT_API_URL=/api
ARG NUXT_AUTH0_DOMAIN
ARG NUXT_AUTH0_CLIENT_ID
ARG NUXT_AUTH0_AUDIENCE

ENV NUXT_API_URL=${NUXT_API_URL} \
    NUXT_AUTH0_DOMAIN=${NUXT_AUTH0_DOMAIN} \
    NUXT_AUTH0_CLIENT_ID=${NUXT_AUTH0_CLIENT_ID} \
    NUXT_AUTH0_AUDIENCE=${NUXT_AUTH0_AUDIENCE}

RUN npm run generate

FROM caddy:2.8-alpine

COPY src/Frontend/Caddyfile /etc/caddy/Caddyfile
COPY --from=build /app/dist/ /srv/

EXPOSE 80
EXPOSE 443
