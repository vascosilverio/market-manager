import React, {useState} from 'react';
import Logo from '../Content/LogoMarketManager.png'; 
import {Link} from "react-router-dom";
import "../CSS_Styles/Navbar.css";
import ReorderIcon from "@mui/icons-material/Reorder"

function Navbar() {

  const [openLinks, setOpenLinks] = useState(false);
  const NavbarToggle = () => {
    setOpenLinks(!openLinks);
  }

  return (
    <div className='navbar'>
      <div className='leftSide' id={openLinks ? "open":"close"}>
        <img src={Logo} alt=""/>
          <div className='hiddenLinks'>
            <Link to="/"> Home </Link>
            <Link to="/bancas"> Bancas </Link>
            <Link to="/reservas"> Reservas </Link>
            <Link to="/vendedores"> Vendedores </Link>
            <Link to="/registo"> Registo </Link>
            <Link to="/login"> Login </Link>
          </div>
        </div>
        <div className='rightSide'>
            <Link to="/"> Home </Link>
            <Link to="/bancas"> Bancas </Link>
            <Link to="/reservas"> Reservas </Link>
            <Link to="/vendedores"> Vendedores </Link>
            <Link to="/registo"> Registo </Link>
            <Link to="/login"> Login </Link>
            <button onClick={NavbarToggle}>  
            <ReorderIcon />
          </button>  
        </div>
    </div>
  );
}

export default Navbar;