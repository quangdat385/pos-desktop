import { useState } from 'react';
import { toast } from 'react-toastify';
import useCart from 'src/hooks/useCart';
import type { Cart, Product } from 'src/types';
import { formatCurrency } from 'src/utils/utils';

interface ProductItemProps {
  product: Product;
}
const ProductItem: React.FC<ProductItemProps> = ({ product }) => {
  const [quantity, setQuantity] = useState<number>(1);
  const { carts, addToCart, removeFromCart } = useCart();
  const addQuantity = () => {
    setQuantity((prev) => (prev < product.quantity ? prev + 1 : prev));
  };
  const getCartQty = (productId: number) => {
    const item = carts.find((i) => i.product_id === productId);
    return item ? item.quantity : 0;
  };
  const reduceQuantity = () => {
    setQuantity((prev) => (prev > 1 ? prev - 1 : 1));
  };
  const reduceCartQuantity = (qty: number) => {
    const currentInCart = getCartQty(product.id);
    if (currentInCart <= 0) {
      return;
    }
    if (currentInCart - qty <= 0) {
      removeFromCart(product.id);
      return;
    }
    const cartItem: Cart = {
      id: product.id,
      product_id: product.id,
      product_name: product.name,
      product_price: product.price,
      quantity: -qty,
      total_price: product.price * (currentInCart - qty)
    };
    addToCart(cartItem);
  };
  const addProductToCart = () => {
    const currentInCart = getCartQty(product.id);
    const newQty = quantity > product.quantity - currentInCart ? product.quantity - currentInCart : quantity;
    if (newQty <= 0) {
      toast.error('Hết hàng trong kho!');
      return;
    }
    const cartItem: Cart = {
      id: product.id,
      product_id: product.id,
      product_name: product.name,
      product_price: product.price,
      quantity: newQty,
      total_price: product.price * newQty
    };
    addToCart(cartItem);
    setQuantity(1);
  };
  return (
    <>
      <div key={product.id} className='border rounded p-3 flex flex-col gap-2'>
        <div>
          <div className='font-medium'>{product.name}</div>
          <div className='text-sm text-gray-600'>{formatCurrency(product.price)}</div>
          <div className='text-xs text-gray-500'>Còn lại: {product.quantity - getCartQty(product.id)}</div>
          <div className='mt-1 text-xs'>
            <span
              id={`incart-${product.id}`}
              className={`ml-0 px-2 py-0.5 rounded-full bg-blue-50 text-blue-700 ${
                getCartQty(product.id) ? '' : 'hidden'
              }`}
            >
              Trong giỏ: {getCartQty(product.id)}
            </span>
          </div>
        </div>
        <div className='mt-auto flex flex-wrap items-center justify-between gap-2'>
          <div className='flex items-center gap-2'>
            <label className='text-sm text-gray-700' htmlFor={`qty-${product.id}`}>
              SL:
            </label>
            <div className='flex items-center gap-1'>
              <button className='px-2 py-1 border rounded cursor-pointer' onClick={reduceQuantity}>
                -
              </button>
              <input
                id={`qty-${product.id}`}
                type='number'
                min='1'
                value={quantity}
                onChange={(e) => setQuantity(Number(e.target.value))}
                className='w-14 text-center border rounded px-1 py-1'
              />
              <button className='px-2 py-1 border rounded cursor-pointer' onClick={addQuantity}>
                +
              </button>
            </div>
          </div>
          <button
            className='bg-green-600 text-white px-3 py-2 rounded flex-1 cursor-pointer'
            onClick={addProductToCart}
          >
            Thêm
          </button>
          <button
            className='bg-green-600 text-white px-3 py-2 rounded flex-1 cursor-pointer'
            onClick={() => reduceCartQuantity(quantity)}
          >
            Bớt
          </button>
        </div>
      </div>
    </>
  );
};
export default ProductItem;
