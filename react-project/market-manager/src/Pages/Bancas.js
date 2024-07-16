import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link, useHistory } from 'react-router-dom';
import { Table, Button } from 'react-bootstrap';

function Bancas() {
  const [bancas, setBancas] = useState([]);
  const [search, setSearch] = useState("");
  const [error, setError] = useState('');
  const history = useHistory();
  const userRole = localStorage.getItem('userRole');

  useEffect(() => {
    const token = localStorage.getItem('token');

    if (!token) {
      history.push('/login');
    } else {
      const fetchBancas = async () => {
        try {
          const response = await axios.get('https://localhost:7172/api/bancas', {
            headers: { Authorization: `Bearer ${token}` }
          });
          console.log('API Response:', response.data);
          if (response.data.success) {
            setBancas(response.data.data.$values);
          } else {
            setError(response.data.message || 'Failed to fetch bancas');
          }
        } catch (error) {
          console.error('Error fetching bancas:', error.response || error);
          setError(error.response?.data?.message || 'An error occurred while fetching bancas');
          if (error.response?.status === 401) {
            history.push('/login');
          }
        }
      };

      fetchBancas();
    }
  }, [history]);

  const filteredBancas = bancas.filter(banca => {
    const searchLower = search.toLowerCase();
    return (
      (banca.BancaId && banca.BancaId.toString().toLowerCase().includes(searchLower)) ||
      (banca.NomeIdentificadorBanca && banca.NomeIdentificadorBanca.toLowerCase().includes(searchLower))
    );
  });

  const getBancaEstadoString = (estadoId) => {
    switch (estadoId) {
      case 0: return 'Ocupada';
      case 1: return 'Livre';
      case 2: return 'Manutenção';
      default: return 'Desconhecido';
    }
  };

  const getBancaCategoriaString = (categoriaId) => {
    switch (categoriaId) {
      case 0: return 'Congelados';
      case 1: return 'Refrigerados';
      case 2: return 'Frescos';
      case 3: return 'Secos';
      case 4: return 'Peixe';
      case 5: return 'Carne';
      default: return 'Desconhecido';
    }
  };

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="Bancas">
      <h1>Bancas</h1>
      <div className="mb-3">
        <input
          type="text"
          placeholder="Pesquisar..."
          className="form-control"
          onChange={e => setSearch(e.target.value)}
        />
      </div>
      {userRole === 'Gestor' && (
        <Button as={Link} to="/criar_banca" variant="primary" className="mb-3">
          Criar Nova Banca
        </Button>
      )}
      {filteredBancas.length > 0 ? (
        <Table striped bordered responsive>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nome Identificador</th>
              <th>Categoria</th>
              <th>Estado Atual</th>
              <th>Localização X</th>
              <th>Localização Y</th>
              <th>Largura</th>
              <th>Comprimento</th>
              <th>Reservas</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {filteredBancas.map((banca) => (
              <tr key={banca.BancaId}>
                <td>{banca.BancaId}</td>
                <td>{banca.NomeIdentificadorBanca}</td>
                <td>{banca.CategoriaBanca !== undefined && getBancaCategoriaString(banca.CategoriaBanca)}</td>
                <td>{banca.EstadoAtualBanca !== undefined && getBancaEstadoString(banca.EstadoAtualBanca)}</td>
                <td>{banca.LocalizacaoX}</td>
                <td>{banca.LocalizacaoY}</td>
                <td>{banca.Largura}</td>
                <td>{banca.Comprimento}</td>
                <td>
                  {banca.Reservas && banca.Reservas.$values && banca.Reservas.$values.map(reserva => (
                    <div key={reserva.ReservaId}>
                      <Link to={`/detalhes_reserva/${reserva.ReservaId}`}>
                        Reserva ID: {reserva.ReservaId}
                      </Link>
                    </div>
                  ))}
                </td>
                <td>
                  <Button as={Link} to={`/detalhes_banca/${banca.BancaId}`} size="sm" variant="info" className="me-2">
                    Detalhes
                  </Button>
                  {userRole === 'Gestor' && (
                    <>
                      <Button as={Link} to={`/editar_banca/${banca.BancaId}`} size="sm" variant="warning" className="me-2">
                        Editar
                      </Button>
                      <Button as={Link} to={`/apagar_banca/${banca.BancaId}`} size="sm" variant="danger">
                        Apagar
                      </Button>
                    </>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      ) : <p className="no-results">Nenhuma banca encontrada.</p>}
    </div>
  );
}

export default Bancas;