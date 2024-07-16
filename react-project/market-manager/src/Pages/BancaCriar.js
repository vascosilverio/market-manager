import React, { useState } from 'react';
import Axios from 'axios';
import '../CSS_Styles/BancasCriar.css';
import { Link } from "react-router-dom";

function BancaCriar(props) {
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

    Axios.post(`https://localhost:7172/api/CreateBanca`, bancaData)
      .then((response) => {
        console.log('Nova banca criada com sucesso:', response.data);
        props.history.push('/bancas'); // Redireciona para a página de listagem de bancas
      })
      .catch((error) => {
        console.error('Erro ao criar nova banca:', error);
      });
  };

  return (
    <div className="BancaCriar">
      <h2>Criar Nova Banca</h2>
      <form onSubmit={handleSubmit}>
        <label>Nome Identificador:</label>
        <input
          type="text"
          value={bancaData.nomeIdentificadorBanca}
          onChange={(e) =>
            setBancaData({ ...bancaData, nomeIdentificadorBanca: e.target.value })
          }
          required
        />
        <label>Categoria:</label>
        <select
          type="number"
          value={bancaData.categoriaBanca}
          onChange={(e) =>{
            setBancaData({ ...bancaData, categoriaBanca: parseInt(e.target.value, 10) })
            console.log(e.target.value);
          }
            
          }
          required
        >
          <option>Escolha uma opção</option>
          <option value={0}>Congelados</option>
          <option value={1}>Refrigerados</option>
          <option value={2}>Frescos</option>
          <option value={3}>Secos</option>
          <option value={4}>Peixe</option>
          <option value={5}>Carne</option>


        </select>
        <label>Largura:</label>
        <input
          type="number"
          value={bancaData.largura}
          onChange={(e) =>
            setBancaData({ ...bancaData, largura: parseInt(e.target.value, 10) })
          }
          required
        />
        <label>Comprimento:</label>
        <input
          type="number"
          value={bancaData.comprimento}
          onChange={(e) =>
            setBancaData({ ...bancaData, comprimento: parseInt(e.target.value, 10) })
          }
          required
        />
        <label>Localização X:</label>
        <input
          type="number"
          value={bancaData.localizacaoX}
          onChange={(e) =>
            setBancaData({ ...bancaData, localizacaoX: parseInt(e.target.value, 10) })
          }
          required
        />
        <label>Localização Y:</label>
        <input
          type="number"
          value={bancaData.localizacaoY}
          onChange={(e) =>
            setBancaData({ ...bancaData, localizacaoY: parseInt(e.target.value, 10) })
          }
          required
        />
        <label>Estado Atual:</label>
        <select
          value={bancaData.estadoAtualBanca}
          onChange={(e) =>
            setBancaData({ ...bancaData, estadoAtualBanca: parseInt(e.target.value, 10) })
          }
          required
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
          required
        />
        <button>Criar Nova Banca</button>
      </form>
      <Link to="/bancas">
        <button> Cancelar </button>
      </Link>
    </div>
  );
}

export default BancaCriar;
