import React, { useState, useEffect } from 'react';
import { useParams, useHistory } from 'react-router-dom';
import { Form, Button } from 'react-bootstrap';
import axios from 'axios';

function EditarReserva() {
  const { id } = useParams();
  const [dataInicio, setDataInicio] = useState('');
  const [dataFim, setDataFim] = useState('');
  const [selectedBancas, setSelectedBancas] = useState([]);
  const [bancas, setBancas] = useState([]);
  const [estadoActual, setEstadoActual] = useState(2);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const history = useHistory();
  const userRole = localStorage.getItem('userRole');

  useEffect(() => {
    const fetchReserva = async () => {
      try {
        const response = await axios.get(`https://localhost:7172/api/reservas/${id}`, {
          headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
        });
        const reserva = response.data;
        setDataInicio(reserva.DataInicio.split('T')[0]);
        setDataFim(reserva.DataFim.split('T')[0]);
        setSelectedBancas(reserva.ListaBancas.$values.map(b => b.BancaId));
        setEstadoActual(reserva.EstadoActualReserva);
      } catch (error) {
        console.error('Error fetching reserva:', error);
        setError('Failed to fetch reserva details');
      }
    };

    const fetchBancas = async () => {
      try {
        const response = await axios.get('https://localhost:7172/api/bancas', {
          headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
        });
        setBancas(response.data.data.$values);
      } catch (error) {
        console.error('Error fetching bancas:', error);
        setError('Failed to fetch bancas');
      }
    };

    fetchReserva();
    fetchBancas();
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.put(`https://localhost:7172/api/reservas/${id}`, {
        dataInicio,
        dataFim,
        selectedBancaIds: selectedBancas,
        estadoActualReserva: userRole === 'Gestor' ? estadoActual : undefined
      }, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      });
      
      setSuccess(true);
      
      if (userRole === 'Gestor' && estadoActual === 0) {
        // Update banca status to Ocupada
        for (const bancaId of selectedBancas) {
          await axios.put(`https://localhost:7172/api/bancas/${bancaId}`, {
            estadoAtualBanca: 0 // Ocupada  
          }, {
            headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
          });
        }
      }
      
      setTimeout(() => {
        history.push('/reservas');
      }, 2000);
    } catch (error) {
      console.error('Error updating reserva:', error);
      setError('Failed to update reserva');
    }
  };

  return (
    <div>
      <h2>Editar Reserva</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label>Data Início</Form.Label>
          <Form.Control type="date" value={dataInicio} onChange={(e) => setDataInicio(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Data Fim</Form.Label>
          <Form.Control type="date" value={dataFim} onChange={(e) => setDataFim(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Bancas</Form.Label>
          {bancas.map((banca) => (
            <Form.Check
              key={banca.BancaId}
              type="checkbox"
              label={banca.NomeIdentificadorBanca}
              checked={selectedBancas.includes(banca.BancaId)}
              onChange={(e) => {
                if (e.target.checked) {
                  setSelectedBancas([...selectedBancas, banca.BancaId]);
                } else {
                  setSelectedBancas(selectedBancas.filter((id) => id !== banca.BancaId));
                }
              }}
            />
          ))}
        </Form.Group>
        {userRole === 'Gestor' && (
          <Form.Group>
            <Form.Label>Estado Actual</Form.Label>
            <Form.Control as="select" value={estadoActual} onChange={e => setEstadoActual(Number(e.target.value))}>
              <option value={0}>Aprovada</option>
              <option value={1}>Recusada</option>  
              <option value={2}>Pendente</option>
              <option value={3}>Concluída</option>
            </Form.Control>
          </Form.Group>
        )}
        {error && <p className="text-danger">{error}</p>}
        {success && <p className="text-success">Reserva atualizada com sucesso!</p>}
        <Button type="submit" disabled={success}>Atualizar Reserva</Button>
      </Form>
    </div>
  );
}

export default EditarReserva;