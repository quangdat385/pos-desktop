interface HeaderProps {
  setIsNewWindowOpen: React.Dispatch<React.SetStateAction<boolean>>;
  setIsEmbeddedVisible: React.Dispatch<React.SetStateAction<boolean>>;
  isRealtimeOnly?: boolean;
  isNewWindowOpen?: boolean;
}

const Header: React.FC<HeaderProps> = ({ setIsNewWindowOpen, setIsEmbeddedVisible, isRealtimeOnly }: HeaderProps) => {
  const openWindow = () => {
    setIsNewWindowOpen((prev) => !prev);
  };

  const toggleEmbedded = () => {
    setIsEmbeddedVisible((prev) => !prev);
  };
  return (
    <div className='flex items-center justify-between mb-4'>
      <h1 className='text-2xl font-bold'>Chào Mừng Đến Với Hệ Thống Bán Hàng Của Chúng Tôi</h1>
      {!isRealtimeOnly && (
        <div className='space-x-2'>
          <button onClick={openWindow} className='bg-purple-600 text-white px-4 py-2 rounded cursor-pointer'>
            Mở Cửa Sổ Mới
          </button>
          <button onClick={toggleEmbedded} className='bg-gray-700 text-white px-4 py-2 rounded cursor-pointer'>
            Hiện/Ẩn Cưa Sổ
          </button>
        </div>
      )}
    </div>
  );
};

export default Header;
