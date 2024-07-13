import React, { useEffect, useState } from 'react';
import Axios from "axios";
import "../CSS_Styles/Bancas.css";

function Bancas() {
  //varivel de estado que armazena o conteúdo da bd com as bancas
  //useState é um hook e banca é a variável de estado atual, sendo que set_banca é a função que a atualiza
  const [banca, set_banca] = useState([]);
  const [search, setSearch] = useState("");
  const apiURL = "https://localhost:7172/api";

  useEffect(() => {
    Axios.get(`${apiURL}/GetAllBancas`).then((response) => {
      console.log(response.data); // Verifique os dados recebidos
      const data = response.data.value; // Acesse o campo "value"
      set_banca(Array.isArray(data) ? data : []); // Garante que os dados são um array
    }).catch((error) => {
      console.error("Erro ao buscar dados da API:", error);
      set_banca([]); // Define um array vazio em caso de erro
    });
  }, []);
  
  const filteredBanca = Array.isArray(banca) ? banca.filter(banca =>
    banca.bancaId.toString().toLowerCase().includes(search.toLowerCase()) ||
    banca.nomeIdentificadorBanca.toLowerCase().includes(search.toLowerCase())
  ) : [];

  return (
    //funcionalidade de pesquisa
    <div className='Banca'>
      <h1 className=''></h1>
      <input
        type="text"
        placeholder="Pesquisar..."
        onChange={e => setSearch(e.target.value)}
      />
      {filteredBanca.length > 0 ? filteredBanca.map((banca, index) => (
        //dar display do conteúdo da bd no ecrã
        <div key={index}>
          <p>Publicação de {banca.bancaId}</p>
          <p className="identificador">Nome Identificador da Banca:</p>
          <p>{banca.nomeIdentificadorBanca}</p>
        </div>
      )) : <p>Loading...</p>}
    </div>
  );
}

// <div key={index}>
// Esta linha começa a definição de um elemento div para cada banca. 
// A propriedade key é usada para dar a cada div uma chave única. 
// É necessária ao criar listas de elementos em React.
// banca.map((banca, index) , banca vai ser o conteúdo da linha da bd
// por isso é que fazemos banca.Nome e etc
// <p>Loading...</p> é para caso não haja conteúdo (ou ainda esteja a ser carregado) ou aconteça alguma
// coisa vá impedir o display do conteúdo, caso isto aconteça, é mostrado "Loading..."

export default Bancas;