import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link, useHistory } from 'react-router-dom';
import { Table, Button } from 'react-bootstrap';

function Reservas() {
  const [reservas, setReservas] = useState([]);
  const [search, setSearch] = useState("");
  const [error, setError] = useState('');
  const history = useHistory();
  const userRole = localStorage.getItem('userRole');
  const userId = localStorage.getItem('userId');

  useEffect(() => {
    const token = localStorage.getItem('token');

    if (!token) {
      history.push('/login');
    } else {
      const fetchReservas = async () => {
        try {
          const response = await axios.get('https://localhost:7172/api/reservas', {
            headers: { Authorization: `Bearer ${token}` }
          });
          console.log('API Response:', response.data);
          if (response.data.success) {
            if (userRole === 'Gestor') {
              setReservas(response.data.data.$values);
            } else {
              setReservas(response.data.data.$values.filter(r => r.UtilizadorId === userId));
            }
          } else {
            setError(response.data.message || 'Failed to fetch reservas');
          }
        } catch (error) {
          console.error('Error fetching reservas:', error.response || error);
          setError(error.response?.data?.message || 'An error occurred while fetching reservas');
          if (error.response?.status === 401) {
            history.push('/login');
          }
        }
      };

      fetchReservas();
    }
  }, [history, userRole, userId]);

  const filteredReservas = reservas.filter(reserva => {
    const searchLower = search.toLowerCase();
    return reserva.ReservaId && reserva.ReservaId.toString().toLowerCase().includes(searchLower);
  });

  const getReservaEstadoString = (estadoId) => {
    switch (estadoId) {
      case 0: return 'Aprovada';
      case 1: return 'Recusada';
      case 2: return 'Pendente';
      case 3: return 'Concluída';
      default: return 'Desconhecido';
    }
  };

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="Reservas">
      <h1>Reservas</h1>
      <div className="mb-3">
        <input
          type="text"
          placeholder="Pesquisar..."
          className="form-control"
          onChange={e => setSearch(e.target.value)}
        />
      </div>
      {(userRole === 'Vendedor' || userRole === 'Gestor') && (
        <Button as={Link} to="/criar_reserva" variant="primary" className="mb-3">
          Criar Nova Reserva
        </Button>
      )}
      {filteredReservas.length > 0 ? (
        <Table striped bordered responsive>
          <thead>
            <tr>
              <th>ID</th>
              <th>Data Início</th>
              <th>Data Fim</th>
              <th>Estado Atual</th>
              <th>Utilizador ID</th>
              <th>Bancas</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {filteredReservas.map((reserva) => (
              <tr key={reserva.ReservaId}>
                <td>{reserva.ReservaId}</td>
                <td>{reserva.DataInicio && new Date(reserva.DataInicio).toLocaleDateString()}</td>
                <td>{reserva.DataFim && new Date(reserva.DataFim).toLocaleDateString()}</td>
                <td>{reserva.EstadoActualReserva !== undefined && getReservaEstadoString(reserva.EstadoActualReserva)}</td>
                <td>{reserva.UtilizadorId}</td>
                <td>
                  {reserva.ListaBancas && reserva.ListaBancas.$values && reserva.ListaBancas.$values.map(banca => (
                    <div key={banca.BancaId}>
                      <Link to={`/detalhes_banca/${banca.BancaId}`}>
                        Banca ID: {banca.BancaId}
                      </Link>
                    </div>
                  ))}
                </td>
                <td>
                  <Button as={Link} to={`/detalhes_reserva/${reserva.ReservaId}`} size="sm" variant="info" className="me-2">
                    Detalhes
                  </Button>
                  {(userRole === 'Gestor' || reserva.UtilizadorId === userId) && (
                    <>
                      <Button as={Link} to={`/editar_reserva/${reserva.ReservaId}`} size="sm" variant="warning" className="me-2">
                        Editar
                      </Button>
                      <Button as={Link} to={`/apagar_reserva/${reserva.ReservaId}`} size="sm" variant="danger">
                        Apagar
                      </Button>
                    </>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      ) : <p className="no-results">Nenhuma reserva encontrada.</p>}
    </div>
  );
}

export default Reservas;