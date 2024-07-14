import React from 'react';
import Axios from "axios";
import "../CSS_Styles/Reservas.css";
import { useParams , Link } from "react-router-dom";

function DeleteBanca({ onClose }) {
  let { reservaId } = useParams();
  const apiURL = "https://localhost:7172/api";

  const handleDelete = () => {
    Axios.delete(`${apiURL}/DeleteReserva?id=${reservaId}`).then((response) => {
      console.log(response.data);
      alert('Reserva eliminada com sucesso!');
      onClose(); // Fecha o modal de confirmação de exclusão após a exclusão bem-sucedida
    }).catch((error) => {
      console.error("Erro ao eliminar a reserva:", error);
      alert('Erro ao eliminar a reserva.');
    });
  };

  return (
    <div className="Barra_delete">
      <h1>Eliminar Banca</h1>
      <p>Tem a certeza de que deseja eliminar a reserva de ID {reservaId}?</p>
      <button className="button" onClick={handleDelete}>Eliminar</button>
      <Link to="/reservas">
        <button className='button'> Cancelar </button>
      </Link>
    </div>
  );
}

export default DeleteBanca;
