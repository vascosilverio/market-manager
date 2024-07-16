import React, { useState, useEffect } from 'react';
import { Link, useHistory } from "react-router-dom";
import ReorderIcon from "@mui/icons-material/Reorder";

function Navbar() {
  const [openLinks, setOpenLinks] = useState(false);
  const history = useHistory();
  const token = localStorage.getItem('token');
  const role = localStorage.getItem('userRole');

  const NavbarToggle = () => {
    setOpenLinks(!openLinks);
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('userRole');
    history.push('/login');
  };

  return (
    <div className='navbar'>
      <div className='leftSide' id={openLinks ? "open" : "close"}>
        <div className='hiddenLinks'>
          {token && role ? (
            <>
              <Link to="/home"> Home </Link>
              {role === 'Gestor' && <Link to="/Bancas"> Bancas </Link>}
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
        {token && role ? (
          <>
            <Link to="/home"> Home </Link>
            {role === 'Gestor' && <Link to="/Bancas"> Bancas </Link>}
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