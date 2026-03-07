FROM oven/bun:1.3.6-alpine AS base
WORKDIR /app
COPY package.json bun.lock ./

FROM base AS dev
RUN bun ci
COPY . .
# storybook
EXPOSE 6006
# vite
EXPOSE 5173 
CMD ["bun", "run", "dev"]

FROM base AS build  
RUN bun ci
COPY . .
RUN bun run build

FROM nginx:alpine AS prod
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

