
export interface Product {
  id: number;
  name: string;
  price: number;
  quantity: number;
  category: string;
}

export type OrderType = {
  id?: number;
  order_number?: string;
  items: OrderItem[];
  total_amount: number;
  created_at?: string;
}
export interface OrderItem {
  id?: number;
  product_id: number;
  product_price: number;
  quantity: number;
  total_price: number;
}


export interface Cart {
  id: number;
  product_id: number;
  product_price: number;
  product_name: string;
  quantity: number;
  total_price: number;
  created_at?: string;
}
export interface Pagination<T> {
  current_page: number;
  page_size: number;
  total_pages: number;
  total_items: number;
  items: T[];
  categories?: string[];
}
export type SuccessResponse<T> = {
  status_code: number;
  message: string;
  data: T;
}
export type ErrorResponse<T> = {
  status_code: number;
  message: string;
  error: T;
}