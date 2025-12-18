import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import useCart from 'src/hooks/useCart';
import type { Cart } from 'src/types';
import { formatCurrency } from 'src/utils/utils';
interface CartItemProps {
  item: Cart;
}
const CartItem: React.FC<CartItemProps> = ({ item }) => {
  const { products } = useCart();
  const [qty, setQty] = useState<number>(0);
  const { addToCart, removeFromCart } = useCart();
  const lineTotal = item.total_price;
  useEffect(() => {
    setQty(item.quantity);
  }, [item.quantity]);
  const removeFromCartFc = (cart_id: number) => {
    removeFromCart(cart_id);
  };
  const increment = () => {
    const payload: Cart = {
      id: item.id,
      product_id: item.product_id,
      product_price: item.product_price,
      product_name: item.product_name,
      quantity: 1,
      total_price: item.product_price * (item.quantity + 1),
      created_at: item.created_at
    };
    const product = products.find((p) => p.id === item.product_id);
    if (product && item.quantity + 1 > product.quantity) {
      toast.error('Số lượng trong giỏ hàng vượt quá số lượng sản phẩm hiện có!');
      return;
    }
    addToCart(payload);
  };
  const decrement = () => {
    if (qty - 1 <= 0) {
      return;
    }
    const payload: Cart = {
      id: item.id,
      product_id: item.product_id,
      product_price: item.product_price,
      product_name: item.product_name,
      quantity: -1,
      total_price: item.product_price * (item.quantity - 1),
      created_at: item.created_at
    };
    addToCart(payload);
  };
  const onQtyInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value;
    const nextQty = Number(val);
    if (Number.isNaN(nextQty)) return;
    const delta = nextQty - item.quantity;
    if (delta === 0) return;
    const payload: Cart = {
      id: item.id,
      product_id: item.product_id,
      product_price: item.product_price,
      product_name: item.product_name,
      quantity: delta,
      total_price: item.product_price * nextQty,
      created_at: item.created_at
    };
    addToCart(payload);
  };
  return (
    <li className='flex items-center justify-between py-2'>
      <div>
        <div className='font-medium'>{item.product_name}</div>
        <div className='text-sm text-gray-600'>Đơn giá: {formatCurrency(item.product_price)}</div>
        <div className='mt-1 flex items-center gap-2'>
          <span className='text-sm'>SL:</span>
          <div className='flex items-center gap-2'>
            <button
              type='button'
              className='px-2 py-1 bg-gray-100 rounded hover:bg-gray-200'
              onClick={decrement}
              aria-label='Giảm số lượng'
            >
              -
            </button>
            <input
              type='number'
              min={1}
              value={qty}
              onChange={onQtyInputChange}
              className='w-16 px-2 py-1 border rounded text-center'
            />
            <button
              type='button'
              className='px-2 py-1 bg-gray-100 rounded hover:bg-gray-200'
              onClick={increment}
              aria-label='Tăng số lượng'
            >
              +
            </button>
          </div>
        </div>
        <div className='text-sm text-gray-600'>
          Thành tiền: <span className='font-medium'>${formatCurrency(lineTotal)}</span>
        </div>
      </div>
      <button className='text-red-600 hover:underline cursor-pointer' onClick={() => removeFromCartFc(item.id)}>
        Xóa
      </button>
    </li>
  );
};
export default CartItem;
