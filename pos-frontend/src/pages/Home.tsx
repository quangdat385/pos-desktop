import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { productsKey, ordersKey } from 'src/constants/queryKeys';
import React, { useEffect, useLayoutEffect, useMemo, useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { createSearchParams, useNavigate } from 'react-router-dom';
import orderApi from 'src/apis/order.api';
import productApi from 'src/apis/product.api';
import Cart from 'src/components/Cart';
import Header from 'src/components/Header';
import Menu from 'src/components/Menu';
import Order from 'src/components/Order';
import Pagination from 'src/components/Pagination';
import ProductList from 'src/components/ProductList';
import path from 'src/constants/path';
import useQueryConfig, { type QueryConfig } from 'src/hooks/useQueryConfig';
import useRefreshQueries from 'src/hooks/useRefreshQueries';
import type { Product } from 'src/types';
import useCart from 'src/hooks/useCart';

const Home: React.FC = () => {
  const { setProducts } = useCart();
  const queryConfig = useQueryConfig();
  const { refetchOrders } = useRefreshQueries();
  const navigate = useNavigate();
  const { data: productData, isSuccess } = useQuery({
    queryKey: productsKey(queryConfig),
    queryFn: () => {
      return productApi.getProducts(queryConfig as QueryConfig);
    },
    placeholderData: keepPreviousData,
    staleTime: 3 * 60 * 1000
  });
  const orderConfig = {
    page: '1',
    limit: '20'
  };
  const { data: ordersData } = useQuery({
    queryKey: ordersKey(orderConfig),
    queryFn: () => {
      return orderApi.getOrders(orderConfig as QueryConfig);
    },
    placeholderData: keepPreviousData,
    staleTime: 3 * 60 * 1000
  });
  useEffect(() => {
    if (isSuccess) {
      setProducts(productData?.data.data.items || []);
    }
  }, [isSuccess, productData, setProducts]);
  const producttFiltered: Product[] = productData?.data.data.items || [];
  const [isNewWindowOpen, setIsNewWindowOpen] = useState<boolean>(false);
  const [isEmbeddedVisible, setIsEmbeddedVisible] = useState<boolean>(true);
  const [indexActive, setIndexActive] = useState<number>(0);
  const [realtimeOnly, setRealtimeOnly] = useState(false);
  const [page, setPage] = useState<number>(1);
  const total_pages = productData?.data.data.total_pages || 1;
  const menus = [...(productData?.data.data.categories || [])];
  useLayoutEffect(() => {
    navigate({
      pathname: path.home,
      search: createSearchParams({
        ...queryConfig,
        page: page.toString(),
        category: menus[indexActive] || 'All'
      }).toString()
    });
  }, [page, indexActive]);

  useLayoutEffect(() => {
    const isRealtimeOnlyParam = queryConfig.realtimeOnly === '1';
    setRealtimeOnly(isRealtimeOnlyParam);
  }, []);
  useEffect(() => {
    if (isNewWindowOpen) {
      const params = new URLSearchParams(window.location.search);
      if (!realtimeOnly) {
        params.set('realtimeOnly', '1');
      } else {
        params.delete('realtimeOnly');
      }
      const newWindow = window.open(
        `${window.location.pathname}?${params.toString()}`,
        '_blank',
        'width=800,height=600'
      );
      const timer = setInterval(() => {
        if (newWindow && newWindow.closed) {
          setIsNewWindowOpen(false);
          clearInterval(timer);
        }
      }, 1000);
      return () => {
        clearInterval(timer);
        if (newWindow && !newWindow.closed) {
          newWindow.close();
        }
      };
    }
  }, [isNewWindowOpen, realtimeOnly]);

  // Listen for cross-window notifications to refetch orders when new order is created
  useEffect(() => {
    let channel: BroadcastChannel | null = null;
    const onMessage = (ev: MessageEvent) => {
      if (ev?.data?.type === 'order-created') {
        refetchOrders();
      }
    };
    if ('BroadcastChannel' in window) {
      channel = new BroadcastChannel('orders');
      channel.addEventListener('message', onMessage);
    }
    return () => {
      if (channel) {
        channel.removeEventListener('message', onMessage);
        channel.close();
      }
    };
  }, [refetchOrders]);
  const gridCols = useMemo(() => (realtimeOnly ? 'grid-cols-1' : 'grid-cols-5'), [realtimeOnly]);
  return (
    <div>
      <Helmet>
        <title>Trang chủ | Dạt Nguyễn Pos</title>
        <meta name='description' content='Trang chủ dự án Dạt Nguyễn Pos' />
      </Helmet>
      <Header
        setIsNewWindowOpen={setIsNewWindowOpen}
        setIsEmbeddedVisible={setIsEmbeddedVisible}
        isRealtimeOnly={realtimeOnly}
      />
      <div className={`grid ${gridCols} gap-4`}>
        <div className={`bg-white p-4 rounded shadow ${realtimeOnly ? 'hidden' : 'col-span-4'}`}>
          <Menu menus={menus} indexActive={indexActive} SetIndexActive={setIndexActive} />
          <ProductList productList={producttFiltered} />
          <Pagination total_pages={total_pages} page={page} setPage={setPage} />
          <Cart />
        </div>
        {isEmbeddedVisible && <Order isRealtimeOnly={realtimeOnly} orders={ordersData?.data.data.items ?? []} />}
      </div>
    </div>
  );
};

export default Home;
