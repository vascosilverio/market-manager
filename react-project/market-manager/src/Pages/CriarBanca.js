import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Form, Button } from 'react-bootstrap';
import axios from 'axios';
import { useAuth } from '../AuthContext';

function CriarBanca() {
  const [nomeIdentificador, setNomeIdentificador] = useState('');
  const [categoria, setCategoria] = useState('');
  const [localizacaoX, setLocalizacaoX] = useState('');
  const [localizacaoY, setLocalizacaoY] = useState('');
  const [largura, setLargura] = useState('');
  const [comprimento, setComprimento] = useState('');
  const [error, setError] = useState('');
  const history = useHistory();
  const { currentUser } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('https://localhost:7172/api/bancas', {
        nomeIdentificadorBanca: nomeIdentificador,
        categoriaBanca: parseInt(categoria),
        localizacaoX: parseInt(localizacaoX),
        localizacaoY: parseInt(localizacaoY),
        largura: parseFloat(largura),
        comprimento: parseFloat(comprimento),
      }, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      });
      history.push('/bancas');
    } catch (error) {
      console.error('Error creating banca:', error);
      setError('Failed to create banca');
    }
  };

  return (
    <div>
      <h2>Criar Nova Banca</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label>Nome Identificador</Form.Label>
          <Form.Control type="text" value={nomeIdentificador} onChange={(e) => setNomeIdentificador(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Categoria</Form.Label>
          <Form.Control as="select" value={categoria} onChange={(e) => setCategoria(e.target.value)} required>
            <option value="">Selecione uma categoria</option>
            <option value="0">Congelados</option>
            <option value="1">Refrigerados</option>
            <option value="2">Frescos</option>
            <option value="3">Secos</option>
            <option value="4">Peixe</option>
            <option value="5">Carne</option>
          </Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Localização X</Form.Label>
          <Form.Control type="number" value={localizacaoX} onChange={(e) => setLocalizacaoX(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Localização Y</Form.Label>
          <Form.Control type="number" value={localizacaoY} onChange={(e) => setLocalizacaoY(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Largura</Form.Label>
          <Form.Control type="number" step="0.01" value={largura} onChange={(e) => setLargura(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Comprimento</Form.Label>
          <Form.Control type="number" step="0.01" value={comprimento} onChange={(e) => setComprimento(e.target.value)} required />
        </Form.Group>
        {error && <p className="text-danger">{error}</p>}
        <Button type="submit">Criar Banca</Button>
      </Form>
    </div>
  );
}

export default CriarBanca;