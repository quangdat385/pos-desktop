import type { QueryConfig } from 'src/hooks/useQueryConfig';
import type { Pagination, Product, SuccessResponse } from 'src/types';
import http from 'src/utils/http';
const URL = 'product';
const productApi = {
  getProducts(params: QueryConfig) {
    return http.get<SuccessResponse<Pagination<Product>>>(`${URL}/get-list-product`, {
      params
    });
  }
};

export default productApi;