import React, { useState, useEffect, useContext } from 'react';
import { Link, useHistory } from "react-router-dom";
import ReorderIcon from "@mui/icons-material/Reorder";
import { useAuth } from '../AuthContext';

function Navbar() {
  const [openLinks, setOpenLinks] = useState(false);
  const { currentUser, logout } = useAuth();
  const history = useHistory();

  const NavbarToggle = () => {
    setOpenLinks(!openLinks);
  };

  const handleLogout = () => {
    logout();
    history.push('/login');
  };

  return (
    <div className='navbar'>
      <div className='leftSide' id={openLinks ? "open" : "close"}>
        {currentUser && (
          <span className="user-role">{currentUser.role}</span>
        )}
        <div className='hiddenLinks'>
          {currentUser ? (
            <>
              <Link to="/home"> Home </Link>
              {currentUser.role === 'Gestor' && <Link to="/Bancas"> Bancas </Link>}
              <Link to="/Reservas"> Reservas </Link>
              <Link to='/' onClick={handleLogout}>Logout</Link>
            </>
          ) : (
            <>
              <Link to='/Login'> Login </Link>
              <Link to="/Register"> Registo </Link>
            </>
          )}
        </div>
      </div>
      <div className='rightSide'>
        {currentUser ? (
          <>
            <Link to="/home"> Home </Link>
            {currentUser.role === 'Gestor' && <Link to="/Bancas"> Bancas </Link>}
            <Link to="/Reservas"> Reservas </Link>
            <Link to='/' onClick={handleLogout}>Logout</Link>
          </>
        ) : (
          <>
            <Link to='/login'> Login </Link>
            <Link to="/Register"> Registo </Link>
          </>
        )}
        <button onClick={NavbarToggle}>
          <ReorderIcon />
        </button>
      </div>
    </div>
  );
}

export default Navbar;