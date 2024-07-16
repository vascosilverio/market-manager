import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { Card, ListGroup, Button } from 'react-bootstrap';
import axios from 'axios';
import { useAuth } from '../AuthContext';

function DetalhesBanca() {
  const { id } = useParams();
  const [banca, setBanca] = useState(null);
  const [error, setError] = useState('');
  const { currentUser } = useAuth();

  useEffect(() => {
    const fetchBanca = async () => {
      try {
        const response = await axios.get(`https://localhost:7172/api/bancas/${id}`, {
          headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
        });
        setBanca(response.data);
      } catch (error) {
        console.error('Error fetching banca:', error);
        setError('Failed to fetch banca details');
      }
    };

    fetchBanca();
  }, [id]);

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

  if (error) return <div>Error: {error}</div>;
  if (!banca) return <div>Loading...</div>;

  return (
    <div>
      <h2>Detalhes da Banca</h2>
      <Card>
        <Card.Body>
          <Card.Title>Banca ID: {banca.bancaId}</Card.Title>
          <ListGroup variant="flush">
            <ListGroup.Item>Nome Identificador: {banca.nomeIdentificadorBanca}</ListGroup.Item>
            <ListGroup.Item>Categoria: {getBancaCategoriaString(banca.categoriaBanca)}</ListGroup.Item>
            <ListGroup.Item>Localização X: {banca.localizacaoX}</ListGroup.Item>
            <ListGroup.Item>Localização Y: {banca.localizacaoY}</ListGroup.Item>
            <ListGroup.Item>Largura: {banca.largura}</ListGroup.Item>
            <ListGroup.Item>Comprimento: {banca.comprimento}</ListGroup.Item>
            <ListGroup.Item>Estado Atual: {getBancaEstadoString(banca.estadoAtualBanca)}</ListGroup.Item>
          </ListGroup>
        </Card.Body>
      </Card>
      {currentUser && currentUser.role === 'Gestor' && (
        <div className="mt-3">
          <Button as={Link} to={`/editar_banca/${banca.bancaId}`} variant="warning" className="me-2">
            Editar
          </Button>
          <Button as={Link} to={`/apagar_banca/${banca.bancaId}`} variant="danger">
            Apagar
          </Button>
        </div>
      )}
    </div>
  );
}

export default DetalhesBanca;