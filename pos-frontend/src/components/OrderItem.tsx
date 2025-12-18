import type { OrderType } from 'src/types';
import { formatCurrency, formatTime } from 'src/utils/utils';

interface OrderItemProps {
  order: OrderType;
}
const OrderItem: React.FC<OrderItemProps> = ({ order }) => {
  const date = new Date(order.created_at as string);
  date.toUTCString();
  return (
    <>
      <li className='py-2'>
        <div className='flex items-center justify-between'>
          <div className='font-medium'>Mã đơn: {order.order_number}</div>
          <div className='text-blue-700 font-semibold'>{formatCurrency(order.total_amount)}</div>
        </div>
        <div className='text-sm text-gray-600'>Thời gian thanh toán: {formatTime(date)}</div>
      </li>
    </>
  );
};
export default OrderItem;
