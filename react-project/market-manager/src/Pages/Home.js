import React from 'react';
import { Link } from 'react-router-dom';
import '../CSS_Styles/Home.css';

function Home() {
  return (
    <>
      <div className="home">
        <div className="headerContainer">
          <h1 className="display-4">Bem Vindo ao Market Manager</h1>
          <h2>Gestor de reservas em bancas de mercado local.</h2>
          <p>Desenvolvido pelos alunos</p>
          <p>Vasco Silvério: Nº22350</p>
          <p>Miguel Brazão: Nº25260</p>
        </div>
      </div>
    </>
  );
}

export default Home;