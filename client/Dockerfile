FROM oven/bun:1.4.0-alpine AS base
WORKDIR /app
COPY package.json bun.lock ./

FROM base AS build  
RUN bun ci
COPY . .
RUN bun run build

FROM nginx:1.31.5-alpine AS prod
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 8080
ENTRYPOINT ["nginx", "-g", "daemon off;"]
