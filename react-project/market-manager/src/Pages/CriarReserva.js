import React, { useState, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { Form, Button, Card } from 'react-bootstrap';
import axios from 'axios';

function CriarReserva() {
  const [dataInicio, setDataInicio] = useState('');
  const [dataFim, setDataFim] = useState('');
  const [selectedBancas, setSelectedBancas] = useState([]);
  const [bancas, setBancas] = useState([]);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const history = useHistory();
  const userId = localStorage.getItem('userId');

  useEffect(() => {
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
    fetchBancas();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('https://localhost:7172/api/reservas', {
        dataInicio,
        dataFim,
        utilizadorId: userId,
        selectedBancaIds: selectedBancas,
        estadoActualReserva: 2 // Pendente
      }, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      });
      setSuccess(true);
      setTimeout(() => {
        history.push('/reservas');
      }, 2000);
    } catch (error) {
      console.error('Error creating reserva:', error);
      setError('Failed to create reserva');
    }
  };

  const handleBancaSelect = (bancaId) => {
    const index = selectedBancas.indexOf(bancaId);
    if (index > -1) {
      setSelectedBancas(selectedBancas.filter(id => id !== bancaId));
    } else {
      setSelectedBancas([...selectedBancas, bancaId]);
    }
  };

  return (
    <div>
      <h2>Criar Nova Reserva</h2>
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
          <div className="banca-cards">{bancas.map((banca) => (
            <Card key={banca.BancaId} className="mb-3">
              <Card.Body>
                <Card.Title>{banca.NomeIdentificadorBanca}</Card.Title>
                <Form.Check
                  type="checkbox"
                  label="Selecionar"
                  checked={selectedBancas.includes(banca.BancaId)}
                  onChange={() => handleBancaSelect(banca.BancaId)}
                />
              </Card.Body>
            </Card>
          ))}
          </div>
        </Form.Group>
        {error && <p className="text-danger">{error}</p>}
        {success && <p className="text-success">Reserva criada com sucesso!</p>}
        <Button type="submit" disabled={success}>Criar Reserva</Button>
      </Form>
    </div>
  );
}
export default CriarReserva;