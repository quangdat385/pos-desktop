import { useQueryClient } from '@tanstack/react-query';
import { QUERY_KEYS } from 'src/constants/queryKeys';

export default function useRefreshQueries() {
  const queryClient = useQueryClient();

  const refreshProducts = () =>
    queryClient.invalidateQueries({ queryKey: [QUERY_KEYS.products] });

  const refreshOrders = () =>
    queryClient.invalidateQueries({ queryKey: [QUERY_KEYS.orders] });

  const refetchProducts = () =>
    queryClient.refetchQueries({ queryKey: [QUERY_KEYS.products] });

  const refetchOrders = () =>
    queryClient.refetchQueries({ queryKey: [QUERY_KEYS.orders] });

  return { refreshProducts, refreshOrders, refetchProducts, refetchOrders };
}
