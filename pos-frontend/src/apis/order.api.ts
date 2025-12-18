import type { QueryConfig } from "src/hooks/useQueryConfig";
import type { OrderType, Pagination, SuccessResponse } from "src/types";
import http from "src/utils/http";

const URL = 'order';
const orderApi = {
  getOrders(params: QueryConfig) {
    return http.get<SuccessResponse<Pagination<OrderType>>>(`${URL}/get-list-order`, {
      params
    });
  },
  createOrder(data: OrderType) {
    return http.post<SuccessResponse<OrderType>>(`${URL}/create`, data);
  }
};
export default orderApi;