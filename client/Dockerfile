FROM oven/bun:1.3.6-alpine AS build
WORKDIR /app
COPY package.json bun.lock ./
RUN bun ci
COPY . .
RUN bun run build

FROM nginx:alpine AS prod
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

