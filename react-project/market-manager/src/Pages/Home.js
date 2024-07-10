import React from 'react';
import {Link} from "react-router-dom";
import HomeVideo from "../Content/HomeVideo.mp4"
import "../CSS_Styles/Home.css";

function Home() {
  return (
    //vídeo de fundo
    <div className="home">
      <video autoPlay loop muted
        style={{
          position: "absolute",
          width: "100%",
          left: "50%",
          top: "50%",
          height: "100%",
          objectFit: "cover",
          transform: "translate(-50%, -50%)",
          zIndex: "-1"
        }}>
        <source src={HomeVideo} type="video/mp4" />
      </video>
      <div className='headerContainer'>
        <h1> MarketManager </h1>
        <p> Gestão e Reserva de Bancas</p>
        <Link to="/shared_trips">
          <button id='HomeButton'> Bancas </button>
        </Link>
      </div>
    </div>
  );
}

export default Home;