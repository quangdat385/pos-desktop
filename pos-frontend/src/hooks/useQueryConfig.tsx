import omitBy from 'lodash/omitBy';
import isUndefined from 'lodash/isUndefined';
import useQueryParams from './useQueryParams';

export type QueryConfig = {
  [key: string]: string;
};

export default function useQueryConfig() {
  const queryParams: QueryConfig = useQueryParams();
  const queryConfig: QueryConfig = omitBy(
    {
      page: queryParams.page || '1',
      limit: queryParams.limit || '20',
      category: queryParams.category || 'All',
      realtimeOnly: queryParams.realtimeOnly || '0'
    },
    isUndefined
  );
  return queryConfig;
}
