import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Sparkles, BookOpen, User, LogOut } from 'lucide-react';

const Navbar = () => {
    const { user, logout, isAuthenticated } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    // No mostrar navbar si no está autenticado
    if (!isAuthenticated()) return null;

    return (
        <nav className="navbar">
            <Link to="/" className="nav-brand">
                <Sparkles size={20} className="icon-primary" /> Aprende+
            </Link>
            <div className="nav-links">
                <Link to="/courses" className="nav-link-item">
                    <BookOpen size={18} /> Mis Cursos
                </Link>
                <div className="nav-user">
                    {user && (
                        <span className="nav-email">
                            <User size={16} /> {user.email}
                        </span>
                    )}
                    <button
                        onClick={handleLogout}
                        className="btn btn-outline btn-sm"
                    >
                        <LogOut size={16} /> Salir
                    </button>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;

