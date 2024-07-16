import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { Card, ListGroup, Button } from 'react-bootstrap';
import axios from 'axios';
import { useAuth } from '../AuthContext';

function DetalhesReserva() {
  const { id } = useParams();
  const [reserva, setReserva] = useState(null);
  const [error, setError] = useState('');
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

  if (error) return <div>Error: {error}</div>;
  if (!reserva) return <div>Loading...</div>;

  return (
    <div>
      <h2>Detalhes da Reserva</h2>
      <Card>
        <Card.Body>
          <Card.Title>Reserva ID: {reserva.reservaId}</Card.Title>
          <ListGroup variant="flush">
            <ListGroup.Item>Data Início: {new Date(reserva.dataInicio).toLocaleDateString()}</ListGroup.Item>
            <ListGroup.Item>Data Fim: {new Date(reserva.dataFim).toLocaleDateString()}</ListGroup.Item>
            <ListGroup.Item>Estado: {reserva.estadoActualReserva}</ListGroup.Item>
            <ListGroup.Item>Utilizador ID: {reserva.utilizadorId}</ListGroup.Item>
          </ListGroup>
        </Card.Body>
      </Card>
      <h3 className="mt-4">Bancas Reservadas</h3>
      <ListGroup>
        {reserva.listaBancas.map((banca) => (
          <ListGroup.Item key={banca.bancaId}>
            Banca ID: {banca.bancaId}, Nome: {banca.nomeIdentificadorBanca}
          </ListGroup.Item>
        ))}
      </ListGroup>
      {(currentUser.role === 'Gestor' || reserva.utilizadorId === currentUser.id) && (
        <div className="mt-3">
          <Button as={Link} to={`/editar_reserva/${reserva.reservaId}`} variant="warning" className="me-2">
            Editar
          </Button>
          <Button as={Link} to={`/apagar_reserva/${reserva.reservaId}`} variant="danger">
            Apagar
          </Button>
        </div>
      )}
    </div>
  );
}

export default DetalhesReserva;