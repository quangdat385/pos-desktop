import React, { createContext, useContext, useEffect, useMemo, useReducer } from 'react';
import type { Cart, Product } from 'src/types';
import { getCartsFromLS, setCartToLS } from 'src/utils/utils';

type CartState = {
  items: Cart[];
  products: Product[];
};

type CartAction =
  | { type: 'ADD_OR_UPDATE'; payload: Cart }
  | { type: 'REMOVE'; payload: { id: number } }
  | { type: 'CLEAR' }
  | { type: 'SET_PRODUCTS'; payload: Product[] };

const initialState: CartState = {
  items: getCartsFromLS(),
  products: []
};

function cartReducer(state: CartState, action: CartAction): CartState {
  switch (action.type) {
    case 'ADD_OR_UPDATE': {
      const cart = action.payload;
      const idx = state.items.findIndex((i) => i.product_id === cart.product_id);
      let next: Cart[];
      if (idx >= 0) {
        next = [...state.items];
        const quantity = next[idx].quantity + cart.quantity;
        next[idx] = {
          ...next[idx],
          quantity: quantity,
          total_price: cart.product_price * quantity,
          created_at: cart.created_at
        };
      } else {
        next = [...state.items, cart];
      }
      return {
        ...state,
        items: next
      };
    }
    case 'REMOVE': {
      return {
        ...state,
        items: state.items.filter((i) => i.id !== action.payload.id)
      };
    }
    case 'CLEAR': {
      return {
        ...state,
        items: []
      };
    }
    case 'SET_PRODUCTS': {
      return {
        ...state,
        products: action.payload
      };
    }
    default:
      return state;
  }
}

type CartContextValue = {
  carts: Cart[];
  addToCart: (cart: Cart) => void;
  removeFromCart: (id: number) => void;
  clearCart: () => void;
  getTotalPrice: () => number;
  products: Product[];
  setProducts: (products: Product[]) => void;
};

const CartContext = createContext<CartContextValue | undefined>(undefined);

export const CartProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [state, dispatch] = useReducer(cartReducer, initialState);

  useEffect(() => {
    setCartToLS(state.items);
  }, [state.items]);

  const value = useMemo<CartContextValue>(
    () => ({
      carts: state.items,
      products: state.products,
      setProducts: (products: Product[]) => dispatch({ type: 'SET_PRODUCTS', payload: products }),
      addToCart: (cart: Cart) => dispatch({ type: 'ADD_OR_UPDATE', payload: cart }),
      removeFromCart: (id: number) => dispatch({ type: 'REMOVE', payload: { id } }),
      clearCart: () => {
        dispatch({ type: 'CLEAR' });
      },
      getTotalPrice: () => state.items.reduce((sum, i) => sum + i.product_price * i.quantity, 0)
    }),
    [state.items]
  );

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
};

export const useCartContext = (): CartContextValue => {
  const ctx = useContext(CartContext);
  if (!ctx) throw new Error('useCartContext must be used within CartProvider');
  return ctx;
};
