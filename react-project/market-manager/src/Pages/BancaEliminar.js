import React from 'react';
import Axios from "axios";
import "../CSS_Styles/Bancas.css";
import { useParams , Link } from "react-router-dom";

function DeleteBanca({ onClose }) {
  let { bancaId } = useParams();
  const apiURL = "https://localhost:7172/api";

  const handleDelete = () => {
    Axios.delete(`${apiURL}/DeleteBanca?id=${bancaId}`).then((response) => {
      console.log(response.data);
      alert('Banca eliminada com sucesso!');
      onClose(); // Fecha o modal de confirmação de exclusão após a exclusão bem-sucedida
    }).catch((error) => {
      console.error("Erro ao eliminar a banca:", error);
      alert('Erro ao eliminar a banca.');
    });
  };

  return (
    <div className="Barra_delete">
      <h1>Eliminar Banca</h1>
      <p>Tem a certeza de que deseja eliminar a banca de ID {bancaId}?</p>
      <button className="button" onClick={handleDelete}>Eliminar</button>
      <Link to="/bancas">
        <button className='button'> Cancelar </button>
      </Link>
    </div>
  );
}

export default DeleteBanca;
