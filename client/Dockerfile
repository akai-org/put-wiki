ARG NODE_VERSION=24.14.0
ARG NGINX_VERSION=1.29.5

FROM node:${NODE_VERSION}-alpine AS base
WORKDIR /app
COPY package.json pnpm-lock.yaml ./
RUN corepack enable && corepack install

FROM base AS build  
RUN pnpm i --frozen-lockfile
COPY . .
RUN pnpm run build

FROM nginx:${NGINX_VERSION}-alpine AS prod
COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 8080
ENTRYPOINT ["nginx", "-g", "daemon off;"]
