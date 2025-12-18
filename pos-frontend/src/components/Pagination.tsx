interface PaginationProps {
  total_pages: number;
  page: number;
  setPage: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({ total_pages, page, setPage }) => {
  return (
    <div className='mt-3 flex items-center justify-between'>
      <button
        onClick={() => setPage(page > 1 ? page - 1 : 1)}
        className={`px-3 py-1 border rounded ${page === 1 ? 'opacity-50 cursor-not-allowed' : ''}`}
      >
        Trước
      </button>
      <span className='text-sm text-gray-700'>
        Trang {page} / {total_pages}
      </span>
      <button
        onClick={() => setPage(page < total_pages ? page + 1 : total_pages)}
        className={`px-3 py-1 border rounded ${page === total_pages ? 'opacity-50 cursor-not-allowed' : ''}`}
      >
        Sau
      </button>
    </div>
  );
};
export default Pagination;
