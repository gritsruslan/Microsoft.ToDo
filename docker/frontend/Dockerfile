FROM node:22-alpine AS build
WORKDIR /app

# For caching dependencies
COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:stable-alpine AS runtime
WORKDIR /usr/share/nginx/html

# Delete default nginx content
RUN rm -rf ./*

COPY --from=build /app/dist/*/browser ./
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80