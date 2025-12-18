import React from 'react';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
    return (
        <nav className="bg-gray-800 p-4">
            <div className="container mx-auto flex justify-between">
                <div className="text-white text-lg font-bold">POS System</div>
                <div>
                    <Link to="/" className="text-gray-300 hover:text-white px-4">Home</Link>
                    <Link to="/sales" className="text-gray-300 hover:text-white px-4">Sales</Link>
                    <Link to="/orders" className="text-gray-300 hover:text-white px-4">Orders</Link>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;