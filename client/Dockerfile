ARG BUN_VERSION=1.3.6
ARG NGINX_VERSION=1.29.5

FROM oven/bun:${BUN_VERSION}-alpine AS base
WORKDIR /app
COPY package.json bun.lock ./

FROM base AS build  
RUN bun ci
COPY . .
RUN bun run build

FROM nginx:${NGINX_VERSION}-alpine AS prod
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 8080
ENTRYPOINT ["nginx", "-g", "daemon off;"]
