import { useEffect, useState } from 'react';
import useCart from '../hooks/useCart';
import { formatCurrency } from 'src/utils/utils';
import CartItem from 'src/components/CartItem';
import { toast } from 'react-toastify';
import type { OrderType } from 'src/types';
import { useMutation } from '@tanstack/react-query';
import orderApi from 'src/apis/order.api';
import useRefreshQueries from 'src/hooks/useRefreshQueries';
interface CartProps {}

const Cart: React.FC<CartProps> = ({}) => {
  const createOrderMutation = useMutation({
    mutationFn: (body: OrderType) => orderApi.createOrder(body)
  });
  const { refetchOrders, refetchProducts } = useRefreshQueries();
  const { carts, getTotalPrice, clearCart } = useCart();
  const [cartList, setCartList] = useState<typeof carts>(carts);
  const checkOut = () => {
    const total = carts.reduce((sum, i) => sum + i.product_price * i.quantity, 0);
    if (carts.length === 0) {
      toast.error('Giỏ hàng trống!');
      return;
    }
    const newOrder = {
      total_amount: total,
      items: carts.map((item) => ({
        product_id: item.product_id,
        product_price: item.product_price,
        quantity: item.quantity,
        total_price: item.total_price
      }))
    };
    createOrderMutation.mutate(newOrder, {
      onSuccess: () => {
        refetchOrders();
        refetchProducts();
        // Notify other app windows/tabs to refetch orders
        try {
          if ('BroadcastChannel' in window) {
            const channel = new BroadcastChannel('orders');
            channel.postMessage({ type: 'order-created' });
            channel.close();
          }
        } catch {
          // noop
        }
        clearCart();
        window.scrollTo({ top: 0, behavior: 'smooth' });
      },
      onError: (error) => {
        toast.error(error.message || 'Thanh toán thất bại. Vui lòng thử lại!');
      }
    });
  };
  useEffect(() => {
    setCartList(carts);
  }, [carts]);
  return (
    <div className='mt-6 border-t pt-4'>
      <h3 className='text-lg font-semibold mb-2'>
        Giỏ hàng{' '}
        <span id='cart-count' className='text-sm text-gray-600 font-normal'>
          ({cartList.length})
        </span>
      </h3>
      <ul className='divide-y'>
        {cartList.map((item) => (
          <CartItem key={item.id} item={item} />
        ))}
      </ul>
      <div className='mt-4 flex items-center justify-between'>
        <h3 className='font-semibold'>
          Tổng: <span id='cart-total'>{formatCurrency(getTotalPrice())}</span>
        </h3>
        <div className='space-x-2'>
          <button onClick={clearCart} className='bg-red-500 text-white px-4 py-2 rounded flex-1 cursor-pointer'>
            Xóa giỏ
          </button>
          <button
            onClick={checkOut}
            className='bg-blue-600 text-white px-4 py-2 rounded cursor-pointer'
            disabled={createOrderMutation.isPending}
          >
            Thanh toán
          </button>
        </div>
      </div>
      <p id='status' className='mt-3 text-green-600 font-medium hidden'>
        Thanh toán thành công!
      </p>
    </div>
  );
};

export default Cart;
