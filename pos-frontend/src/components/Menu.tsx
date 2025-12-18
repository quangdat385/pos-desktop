interface MenuProps {
  menus: string[];
  indexActive: number;
  SetIndexActive: React.Dispatch<React.SetStateAction<number>>;
}
const Menu: React.FC<MenuProps> = ({ menus, indexActive, SetIndexActive }) => {
  return (
    <div id='category-filter' className='mb-3 flex flex-wrap gap-2'>
      {menus.map((menu, idx) => (
        <button
          key={idx}
          className={`px-3 py-1 rounded border ${
            indexActive === idx ? 'bg-blue-600 text-white border-blue-600' : 'bg-white text-gray-700'
          }`}
          onClick={() => SetIndexActive(idx)}
        >
          {menu}
        </button>
      ))}
    </div>
  );
};
export default Menu;
