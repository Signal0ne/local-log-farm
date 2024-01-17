# Stage 1: Build the Angular app
FROM node:19.5.0-alpine AS builder
WORKDIR /app
COPY package.json package-lock.json ./
RUN NODE_ENV=development npm install
COPY . .
RUN NODE_ENV=development npm run build

# Stage 2: Setup Nginx to serve the built Angular app
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
