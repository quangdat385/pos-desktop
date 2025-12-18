import React, { useEffect } from 'react';
import { HelmetProvider } from 'react-helmet-async';
import { ToastContainer } from 'react-toastify';
import ErrorBoundary from 'src/components/ErrorBoundary';
import useCart from 'src/hooks/useCart';
import useRouteElements from 'src/useRouterElenments';
import { LocalStorageEventTarget } from 'src/utils/utils';
const App: React.FC = () => {
  const routeElements = useRouteElements();
  const { clearCart } = useCart();
  useEffect(() => {
    LocalStorageEventTarget.addEventListener('clearLS', clearCart);
    return () => {
      LocalStorageEventTarget.removeEventListener('clearLS', clearCart);
    };
  }, [clearCart]);
  return (
    <div className='min-h-screen bg-blue-100'>
      <HelmetProvider>
        <ErrorBoundary>
          {routeElements}
          <ToastContainer />
        </ErrorBoundary>
      </HelmetProvider>
    </div>
  );
};

export default App;
