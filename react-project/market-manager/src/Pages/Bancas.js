import React, { useEffect, useState } from 'react';
import "../CSS_Styles/Bancas.css";

function Bancas() {
  const [banca, setBanca] = useState([]);
  const [search, setSearch] = useState("");
  const apiURL = "http://localhost:7172/api/Api/";

  useEffect(() => {
    fetch(`${apiURL}/GetAllBancas`)
      .then(response => response.json())
      .then(data => setBanca(data))
      .catch(error => console.error('Erro ao buscar bancas:', error));
  }, []);

  const filteredBanca = banca.filter(banca =>
    banca.bancaId.toLowerCase().includes(search.toLowerCase()) ||
    banca.nomeIdentificadorBanca.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <div className='Banca'>
      <h1 className=''></h1>
      <input
        type="text"
        placeholder="Pesquisar..."
        onChange={e => setSearch(e.target.value)}
      />
      {filteredBanca.length > 0 ? filteredBanca.map((banca, index) => (
        <div key={index}>
          <p>Publicação de {banca.bancaId}</p>
          <p className="identificador">Nome Identificador da Banca:</p>
          <p>{banca.nomeIdentificadorBanca}</p>
        </div>
      )) : <p>Loading...</p>}
    </div>
  );
}

export default Bancas;
