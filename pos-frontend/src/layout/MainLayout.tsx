import { memo } from 'react';
import { Outlet } from 'react-router-dom';
interface Props {
  children?: React.ReactNode;
}
function MainLayoutInner({ children }: Props) {
  return (
    <div className='mx-auto p-6 xxl:container'>
      {children}
      <Outlet />
    </div>
  );
}
const MainLayout = memo(MainLayoutInner);
export default MainLayout;
