import { useCartContext } from 'src/contexts/CartContext';

const useCart = () => {
  return useCartContext();
};

export default useCart;