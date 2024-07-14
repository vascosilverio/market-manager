import React, { useState, useEffect } from 'react';
import Axios from 'axios';
import '../CSS_Styles/BancasEditar.css';
import { useParams , Link } from "react-router-dom";

function BancasEditar(props) {
  let { bancaId } = useParams();
  const [bancaData, setBancaData] = useState({
    nomeIdentificadorBanca: '',
    categoriaBanca: 0,
    largura: 0,
    comprimento: 0,
    localizacaoX: 0,
    localizacaoY: 0,
    estadoAtualBanca: 0,
    fotografiaBanca: '',
  });

  const handleSubmit = (e) => {
    e.preventDefault();

    Axios.put(`https://localhost:7172/api/UpdateBanca?id=${bancaId}`, bancaData)
      .then((response) => {
        console.log('Dados da banca atualizados com sucesso:', response.data);
        props.history.push('/bancas'); // Redireciona para a página de listagem de bancas
      })
      .catch((error) => {
        console.error('Erro ao atualizar dados da banca:', error);
      });
  };

  return (
    <div className="BancaEditar">
      <h2>Editar Banca de ID {bancaId}</h2>
      <form onSubmit={handleSubmit}>
        <label>Nome Identificador:</label>
        <input
          type="text"
          value={bancaData.nomeIdentificadorBanca}
          onChange={(e) =>
            setBancaData({ ...bancaData, nomeIdentificadorBanca: e.target.value })
          }
        />
        <label>Categoria:</label>
        <input
          type="number"
          value={bancaData.categoriaBanca}
          onChange={(e) =>
            setBancaData({ ...bancaData, categoriaBanca: parseInt(e.target.value, 10) })
          }
        />
        <label>Largura:</label>
        <input
          type="number"
          value={bancaData.largura}
          onChange={(e) =>
            setBancaData({ ...bancaData, largura: parseInt(e.target.value, 10) })
          }
        />
        <label>Comprimento:</label>
        <input
          type="number"
          value={bancaData.comprimento}
          onChange={(e) =>
            setBancaData({ ...bancaData, comprimento: parseInt(e.target.value, 10) })
          }
        />
        <label>Localização X:</label>
        <input
          type="number"
          value={bancaData.localizacaoX}
          onChange={(e) =>
            setBancaData({ ...bancaData, localizacaoX: parseInt(e.target.value, 10) })
          }
        />
        <label>Localização Y:</label>
        <input
          type="number"
          value={bancaData.localizacaoY}
          onChange={(e) =>
            setBancaData({ ...bancaData, localizacaoY: parseInt(e.target.value, 10) })
          }
        />
        <label>Estado Atual:</label>
        <select
          value={bancaData.estadoAtualBanca}
          onChange={(e) =>
            setBancaData({ ...bancaData, estadoAtualBanca: parseInt(e.target.value, 10) })
          }
        >
          <option value={0}>Ocupada</option>
          <option value={1}>Livre</option>
          <option value={2}>Em Manutenção</option>
        </select>
        <label>Fotografia:</label>
        <input
          type="text"
          value={bancaData.fotografiaBanca}
          onChange={(e) =>
            setBancaData({ ...bancaData, fotografiaBanca: e.target.value })
          }
        />
        <button>Salvar Alterações</button>
        <Link to="/bancas">
            <button> Cancelar </button>
        </Link>
      </form>
    </div>
  );
}

export default BancasEditar;
