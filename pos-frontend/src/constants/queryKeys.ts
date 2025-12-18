import type { QueryConfig } from "src/hooks/useQueryConfig";

export const QUERY_KEYS = {
  products: 'products',
  orders: 'orders'
} as const;

export const productsKey = (params: QueryConfig) => [QUERY_KEYS.products, params];
export const ordersKey = (params: QueryConfig) => [QUERY_KEYS.orders, params];
