# --- Build Stage ---
FROM node:22.16-alpine AS builder

WORKDIR /app

COPY package*.json ./
COPY package-lock.json ./
RUN npm install --legacy-peer-deps

COPY . .

RUN npm run build

# --- Production Stage ---
FROM nginx:alpine

# Copy build output to Nginx html directory
COPY --from=builder /app/dist /usr/share/nginx/html
# Copy environment file if exists
COPY .env /usr/share/nginx/html/.env
# Copy custom Nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copy custom Nginx config if needed
# COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 4000

CMD ["nginx", "-g", "daemon off;"]