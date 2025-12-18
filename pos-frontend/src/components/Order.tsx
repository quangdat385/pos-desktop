import { memo, type JSX } from 'react';
import OrderItem from 'src/components/OrderItem';
import type { OrderType } from 'src/types';

interface OrderProps {
  isRealtimeOnly: boolean;
  orders: OrderType[];
}

const OrderInner: React.FC<OrderProps> = ({ isRealtimeOnly, orders }: OrderProps) => {
  let ordersElements: JSX.Element | JSX.Element[] | null = null;
  if (orders.length === 0) {
    ordersElements = <li className='py-2 text-gray-500'>Chưa có đơn hàng.</li>;
  } else {
    ordersElements = orders.map((order) => <OrderItem key={order.id} order={order} />);
  }
  return (
    <div
      className={`bg-white p-4 rounded shadow ${
        isRealtimeOnly ? 'col-span-3 w-full' : 'col-span-1'
      } overflow-auto min-h-screen`}
    >
      <h2 className='text-xl font-semibold mb-3'>Đơn hàng</h2>
      <div className='text-sm text-gray-600 mb-3'>Tự động cập nhật khi có đơn hàng mới.</div>
      <ul id='orders-list' className='divide-y'>
        {ordersElements}
      </ul>
    </div>
  );
};
const Order = memo(OrderInner);
export default Order;
