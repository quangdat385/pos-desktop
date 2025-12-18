# POS Frontend (React + TanStack Query)

Ứng dụng POS frontend cho bán hàng, hỗ trợ mở nhiều cửa sổ, xem đơn theo thời gian thực và đồng bộ khi tạo đơn.

## Công nghệ chính

- React, React Router
- @tanstack/react-query
- react-helmet-async
- TypeScript
- Vite (build ra thư mục `dist`)
- Nginx (serve bản build trong Docker)

## Cấu trúc thư mục

```
pos-frontend/
├─ Dockerfile
├─ nginx.conf
├─ package.json
├─ src/
│  ├─ apis/
│  │  ├─ order.api.ts
│  │  └─ product.api.ts
│  ├─ components/
│  │  ├─ Cart.tsx
│  │  ├─ Header.tsx
│  │  ├─ Menu.tsx
│  │  ├─ Order.tsx
│  │  ├─ Pagination.tsx
│  │  └─ ProductList.tsx
│  ├─ constants/
│  │  ├─ path.ts
│  │  └─ queryKeys.ts
│  ├─ hooks/
│  │  ├─ useCart.ts
│  │  ├─ useQueryConfig.ts
│  │  └─ useRefreshQueries.ts
│  ├─ pages/
│  │  └─ Home.tsx
│  └─ types/
│     └─ (Product, …)
└─ ...
```

## Thiết lập và chạy

- Yêu cầu: Node 18/20 LTS, npm 9+
- Cài deps:

```
npm install --legacy-peer-deps
```

- Chạy dev:

```
npm run dev
```

- Build production:

```
npm run build
```

- Xem thử bản build:

```
npm run preview
```

## Docker

Build và chạy container Nginx phục vụ thư mục `dist`:

```
docker build --progress=plain -t quangdat385/pos-frontend:v0 .
docker run -dp 4000:80 --name pos-frontend --restart unless-stopped quangdat385/pos-frontend:v0
```

Truy cập: http://localhost:4000

Lưu ý tương thích dependency khi build Docker:

- Nếu lỗi native module trên Alpine (esbuild/sharp/canvas…):
  - Dùng Node 20 thay vì 22, hoặc
  - Chuyển base image build sang Debian (`node:20-bookworm-slim`), hoặc
  - Cài thêm `libc6-compat` (Alpine) và toolchain (`python3 make g++`).
- Nếu lỗi peer-deps:
  - Ưu tiên `npm ci` (có `package-lock.json`), nếu không thì `npm install --legacy-peer-deps`.

Ví dụ build stage an toàn:

- Alpine: cài `libc6-compat`, `python3 make g++`, Node 20.
- Debian slim: tránh vấn đề musl của Alpine.

## Luồng dữ liệu chính

- Products: useQuery với `productsKey(queryConfig)`, lưu danh sách vào context Cart bằng `useCart().setProducts` trong `useEffect`.
- Orders: useQuery với `ordersKey({ page, limit })`.
- Tạo đơn: mutation createOrder; onSuccess:
  - `invalidateQueries(ordersKey(...))` để refetch tại cửa sổ hiện tại.
  - Phát sự kiện cross-window:
    - `BroadcastChannel('orders').postMessage({ type: 'order-created' })`
- Nhận sự kiện: `Home.tsx` lắng nghe kênh `orders` và `storage` để gọi `refetchOrders()`.

## Mở cửa sổ chỉ hiển thị Realtime Order

- Nút mở cửa sổ mới sẽ thêm/thay đổi `?realtimeOnly=1`.
- Cửa sổ mới chỉ hiển thị danh sách đơn; sẽ tự refetch khi đơn mới được tạo ở cửa sổ khác.
