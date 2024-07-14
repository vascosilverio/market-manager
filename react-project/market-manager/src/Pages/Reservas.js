import React, { useEffect, useState } from 'react';
import Axios from "axios";
import "../CSS_Styles/Reservas.css";
import {Link} from "react-router-dom";
import ReservaFoto from '../Content/Banca.jpg';

function Reservas() {
  //varivel de estado que armazena o conteúdo da bd com as Reservas
  //useState é um hook e Reserva é a variável de estado atual, sendo que set_reserva é a função que a atualiza
  const [reserva, set_reserva] = useState([]);
  const [search, setSearch] = useState("");
  const apiURL = "https://localhost:7172/api";

  useEffect(() => {
    Axios.get(`${apiURL}/GetAllReservas`).then((response) => {
      console.log(response.data); // Verifique os dados recebidos
      const data = response.data.value; // Acesse o campo "value"
      set_reserva(Array.isArray(data) ? data : []); // Garante que os dados são um array
    }).catch((error) => {
      console.error("Erro ao buscar dados da API:", error);
      set_reserva([]); // Define um array vazio em caso de erro
    });
  }, []);
  
  const filteredReserva = Array.isArray(reserva) ? reserva.filter(reserva =>
    reserva.reservaId.toString().toLowerCase().includes(search.toLowerCase()) ||
    reserva.utilizador.toLowerCase().includes(search.toLowerCase())
  ) : [];

  return (
    //funcionalidade de pesquisa
    <div className='Reservas'>
      <h1 className=''></h1>
      <input
        type="text"
        placeholder="Pesquisar..."
        onChange={e => setSearch(e.target.value)}
      />
      {filteredReserva.length > 0 ? filteredReserva.map((reserva, index) => (
        //dar display do conteúdo da bd no ecrã
        <div key={index}>
          <img className='ReservaFoto' src={ReservaFoto} />
            <div className="ReservaDetails">
              <p>ID da Reserva: {reserva.reservaId}</p>
              <p>Data Início: {reserva.dataInicio}</p>
              <p>Data Fim: {reserva.dataFim}</p>
              <p>Utilizador: {reserva.utilizadorId}</p>
              {reserva.estadoActualReserva === 0 && <p>Estado Atual da Reserva: Aprovada</p>}
              {reserva.estadoActualReserva === 1 && <p>Estado Atual da Reserva: Recusada</p>}
              {reserva.estadoActualReserva === 2 && <p>Estado Atual da Reserva: Pendente</p>}
              {reserva.estadoActualReserva === 3 && <p>Estado Atual da Reserva: Concluída</p>}
              <p>Reservas: {reserva.selectedReservaIds}</p>
            </div>
            <div className="ReservaButtons">
              <Link to={`/eliminar_reserva/${reserva.reservaId}`}>
                <button className='button' > Eliminar </button>
              </Link>  
            </div> 
        </div>    
      )) : <p>Loading...</p>}
      <div id="footer" className='footer'></div> {/* Este elemento representa o footer */}
    </div>
  );
}

// <div key={index}>
// Esta linha começa a definição de um elemento div para cada Reserva. 
// A propriedade key é usada para dar a cada div uma chave única. 
// É necessária ao criar listas de elementos em React.
// Reserva.map((Reserva, index) , Reserva vai ser o conteúdo da linha da bd
// por isso é que fazemos Reserva.Nome e etc
// <p>Loading...</p> é para caso não haja conteúdo (ou ainda esteja a ser carregado) ou aconteça alguma
// coisa vá impedir o display do conteúdo, caso isto aconteça, é mostrado "Loading..."

export default Reservas;