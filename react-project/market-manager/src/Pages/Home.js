import React from 'react';
import { Link } from 'react-router-dom';
import '../CSS_Styles/Home.css';

function Home() {
  const userRole = localStorage.getItem('userRole');

  return (
    <>
      <div className="home">
        <div className="headerContainer">
          <h1 className="display-4">Bem Vindo ao Market Manager</h1>
          <h2>Gestor de reservas em bancas de mercado local.</h2>
          <p>Desenvolvido pelos alunos</p>
          <p>Vasco Silvério: Nº22350</p>
          <p>Miguel Brazão: Nº25260</p>
          {userRole && (
            <div>
              <Link to="/bancas" className="btn btn-primary me-2">Bancas</Link>
              <Link to="/reservas" className="btn btn-primary">Reservas</Link>
            </div>
          )}
        </div>
      </div>
    </>
  );
}

export default Home;