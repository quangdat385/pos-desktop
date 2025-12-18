# Chạy Local

<!-- front end -->

```
cd pos-frontend
npm i --legacy-peer-deps
npm run dev
```

<!-- back end -->

```
cd PosApi
dotnet build
dotnet run
```

## Chạy Docker

```
docker compose -f docker-compose.yml up -d
docker compose -f docker-compose.yml down -v

```
