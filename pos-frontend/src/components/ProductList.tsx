import ProductItem from 'src/components/ProductItem';
import type { Product } from 'src/types';

interface ProductListProps {
  productList: Product[];
}
const ProductList: React.FC<ProductListProps> = ({ productList }: ProductListProps) => {
  return (
    <div id='product-list' className='grid grid-cols-5 gap-3'>
      {productList.map((p) => (
        <ProductItem key={p.id} product={p} />
      ))}
    </div>
  );
};

export default ProductList;
