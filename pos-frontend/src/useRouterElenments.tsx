import { useRoutes } from 'react-router-dom';
import { Suspense, lazy } from 'react';
import MainLayout from 'src/layout/MainLayout';
import path from 'src/constants/path';

const Home = lazy(() => import('src/pages/Home'));

export default function useRouteElements() {
  const routeElements = useRoutes([
    {
      path: '',
      element: <MainLayout />,
      children: [
        {
          path: path.home,
          element: (
            <Suspense>
              <Home />
            </Suspense>
          )
        }
      ]
    }
  ]);

  return routeElements;
}
