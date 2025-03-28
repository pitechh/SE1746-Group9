# Stage 1: Build React App
FROM node:18 AS build
WORKDIR /app

# Copy package.json và cài đặt dependencies
COPY package.json package-lock.json ./
RUN npm install --legacy-peer-deps

# Copy toàn bộ mã nguồn và build
COPY . .
RUN npm run build

# Stage 2: Serve React App bằng serve
FROM node:18
WORKDIR /app

# Cài đặt serve để chạy ứng dụng
RUN npm install -g serve

# Copy build từ stage 1 sang
COPY --from=build /app/build /app/build

# Mở cổng 3000
EXPOSE 3000

# Chạy ứng dụng với serve
CMD ["serve", "-s", "build", "-l", "3000"]
