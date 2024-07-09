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
            <Link to="/shared_trips"> Shared Trips </Link>
            <Link to="/share_your_trip"> Share Your Trip </Link>
            <Link to="/about_us"> About us </Link>
            <Link to="/admin_dashboard"> Admin Dashboard </Link>
          </div>
        </div>
        <div className='rightSide'>
            <Link to="/"> Home </Link>
            <Link to="/shared_trips"> Shared Trips </Link>
            <Link to="/share_your_trip"> Share Your Trip </Link>
            <Link to="/about_us"> About us </Link>
            <Link to="/admin_dashboard"> Admin Dashboard </Link>
            <button onClick={NavbarToggle}>  
            <ReorderIcon />
          </button>  
        </div>
    </div>
  );
}

export default Navbar;