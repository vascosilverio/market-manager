import React, { useState, useEffect } from 'react';
import { useParams, useHistory } from 'react-router-dom';
import { Card, Button } from 'react-bootstrap';
import axios from 'axios';
import { useAuth } from '../AuthContext';

function ApagarReserva() {
  const { id } = useParams();
  const [reserva, setReserva] = useState(null);
  const [error, setError] = useState('');
  const history = useHistory();
  const { currentUser } = useAuth();

  useEffect(() => {
    const fetchReserva = async () => {
      try {
        const response = await axios.get(`https://localhost:7172/api/reservas/${id}`, {
          headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
        });
        setReserva(response.data);
      } catch (error) {
        console.error('Error fetching reserva:', error);
        setError('Failed to fetch reserva details');
      }
    };

    fetchReserva();
  }, [id]);

  const handleDelete = async () => {
    try {
      await axios.delete(`https://localhost:7172/api/reservas/${id}`, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      });
      history.push('/reservas');
    } catch (error) {
      console.error('Error deleting reserva:', error);
      setError('Failed to delete reserva');
    }
  };

  if (error) return <div>Error: {error}</div>;
  if (!reserva) return <div>Loading...</div>;

  return (
    <div>
      <h2>Apagar Reserva</h2>
      <Card>
        <Card.Body>
          <Card.Title>Tem certeza que deseja apagar esta reserva?</Card.Title>
          <Card.Text>
            Reserva ID: {reserva.reservaId}<br />
            Data Início: {new Date(reserva.dataInicio).toLocaleDateString()}<br />
            Data Fim: {new Date(reserva.dataFim).toLocaleDateString()}
          </Card.Text>
          <Button variant="danger" onClick={handleDelete}>Confirmar Exclusão</Button>
          <Button variant="secondary" onClick={() => history.goBack()} className="ms-2">Cancelar</Button>
        </Card.Body>
      </Card>
    </div>
  );
}

export default ApagarReserva;