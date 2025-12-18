# PosApi (ASP.NET Core .NET 8)

API cho hệ thống POS, sử dụng ASP.NET Core (.NET 8), Entity Framework Core (MySQL), Serilog và Swagger.

## Cấu trúc dự án

```
PosApi/
├─ appsettings.json
├─ appsettings.Development.json
├─ PosApi.csproj
├─ Program.cs
├─ Dockerfile
├─ .dockerignore
├─ Properties/
│  └─ launchSettings.json
├─ Migrations/
│  └─ ... (các file migration EF Core)
└─ src/
   ├─ Configurations/
   ├─ Controllers/
   │  ├─ OrderController.cs
   │  └─ ProductController.cs
   ├─ Data/
   │  └─ AppDbContext.cs
   ├─ DTOs/
   ├─ Entities/
   │  ├─ Order.cs
   │  ├─ OrderItem.cs
   │  └─ Product.cs
   ├─ Exceptions/
   ├─ Extensions/
   │  ├─ ServiceCollectionExtensions.cs
   │  └─ WebApplicationExtensions.cs
   ├─ Infrastructure/
   ├─ Middlewares/
   ├─ Repositories/
   ├─ Seeders/
   └─ Services/
```

## Yêu cầu hệ thống

-   .NET SDK 8.0
-   MySQL 8.x đang chạy (local hoặc remote)
-   (Tùy chọn) Docker Desktop nếu chạy bằng Docker

## Cấu hình kết nối CSDL

Mặc định đọc từ `ConnectionStrings:DefaultConnection` trong `appsettings.json`. Có thể override bằng biến môi trường:

-   Windows (PowerShell):

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Port=33306;Database=test_vn;User=root;Password=test_vn;SslMode=None;AllowPublicKeyRetrieval=True;Pooling=true;Min Pool Size=2;Max Pool Size=10;"
```

-   Khi chạy trong Docker (khuyến nghị dùng host.docker.internal để trỏ về MySQL trên máy host):

```
ConnectionStrings__DefaultConnection=Server=host.docker.internal;Port=3306;Database=test_vn;User=root;Password=your_password;SslMode=None;AllowPublicKeyRetrieval=True;Pooling=true;Min Pool Size=2;Max Pool Size=10;
```

> Lưu ý: Ứng dụng sẽ tự động `Migrate` và `Seed` dữ liệu mẫu ở lần chạy đầu (xem `Program.cs`).

## Khởi tạo & Chạy (Local)

Các URL mặc định khi chạy local (theo `launchSettings.json`):

-   HTTP: http://localhost:5220
-   HTTPS: https://localhost:7224

Các bước:

```powershell
# Tại thư mục PosApi
cd PosApi

# Khôi phục package
dotnet restore

# (Tuỳ chọn) Cài dotnet-ef nếu muốn thao tác migrate thủ công
# dotnet tool install --global dotnet-ef

# Chạy ứng dụng (sẽ tự migrate + seed nếu kết nối DB OK)
dotnet run
```

Mở Swagger UI tại: `http://localhost:5220/swagger` (hoặc URL hiện trên console khi chạy).

### Lệnh EF Core tham khảo (tuỳ chọn)

```powershell
# Tạo migration mới
# dotnet ef migrations add <MigrationName>

# Áp dụng migration vào DB
# dotnet ef database update
```

## Chạy bằng Docker

Dockerfile đã cấu hình để container lắng nghe cổng 8080 và chạy ở `ASPNETCORE_ENVIRONMENT=Production` theo mặc định.

### Build image

```bash
# Từ thư mục PosApi
docker build --progress=plain -t quangdat385/pos-backend:v0 .
```

### Run container

```bash
# Map cổng host 8080 -> container 8080
# Truyền biến môi trường cho connection string (sửa password/DB tuỳ bạn)
docker run --rm -dp 88080:8080
  -e ASPNETCORE_ENVIRONMENT=Development
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Port=3306;Database=test_vn;User=root;Password=783151dat;SslMode=None;AllowPublicKeyRetrieval=True;Pooling=true;Min Pool Size=2;Max Pool Size=10;"
  quangdat385/pos-backend:v0
```

Truy cập: `http://localhost:8080/swagger`.

### docker-compose (tuỳ chọn)

Thêm service sau vào `docker-compose.yml` ở thư mục gốc (hoặc dùng file compose riêng):

```yaml
services:
    posapi:
        build:
            context: ./PosApi
            dockerfile: Dockerfile
        ports:
            - "8080:8080"
        environment:
            ASPNETCORE_ENVIRONMENT: Development
            ConnectionStrings__DefaultConnection: >-
                Server=host.docker.internal;Port=3306;Database=test_vn;User=root;Password=your_password;SslMode=None;AllowPublicKeyRetrieval=True;Pooling=true;Min Pool Size=2;Max Pool Size=10;
```

## Endpoint chính (tham khảo)

-   `/swagger` – Tài liệu API (Swagger UI)
-   Product: `/api/v1/product`
-   Order: `/api/v1/order`

## Gỡ lỗi nhanh

-   Không kết nối được MySQL: kiểm tra host, user/password, quyền truy cập, và port 3306. Với Docker Desktop trên Windows, dùng `host.docker.internal` để container kết nối về MySQL trên máy host.
-   Port bận: đổi cổng host (ví dụ `-p 9090:8080`) hoặc đóng tiến trình đang chiếm cổng.
-   Migration lỗi: đảm bảo DB tồn tại và user có quyền; có thể chạy `dotnet ef database update` thủ công để xem log chi tiết.
