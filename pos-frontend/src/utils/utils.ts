import type { Cart } from "src/types";

export const formatCurrency = (amount: number): string => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND',
  }).format(amount);
};
export const formatTime = (date: Date): string =>
  new Intl.DateTimeFormat('vi-VN', {
    timeZone: 'Asia/Ho_Chi_Minh',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    year: 'numeric',
    month: '2-digit',
    day: '2-digit'
  }).format(date);

export const LocalStorageEventTarget = new EventTarget();
export const setCartToLS = (carts: Cart[]) => {
  localStorage.setItem('carts', JSON.stringify(carts));
}
export const removeFromCartsLS = (id: number) => {
  const carts: Cart[] = JSON.parse(localStorage.getItem('carts') || '[]');
  const updatedCarts = carts.filter((c) => c.id !== id);
  localStorage.setItem('carts', JSON.stringify(updatedCarts));
}
export const getCartsFromLS = (): Cart[] => {
  const result = localStorage.getItem('carts');
  return result ? JSON.parse(result) : [];
};
export const dispatchClearLSEvent = () => {
  localStorage.removeItem('carts');
  const clearLSEvent = new Event('clearLS');
  LocalStorageEventTarget.dispatchEvent(clearLSEvent);
};
