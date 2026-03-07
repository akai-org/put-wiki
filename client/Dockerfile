ARG BUN_VERSION=1.3.6
FROM oven/bun:${BUN_VERSION}-alpine AS base
WORKDIR /app
COPY package.json bun.lock ./

FROM base AS build  
RUN bun ci
COPY . .
RUN bun run build

FROM nginx:alpine AS prod
COPY nginx.conf /etc/nginx/conf.d/
COPY --from=build /app/dist /usr/share/nginx/html
RUN rm /etc/nginx/conf.d/default.conf
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
