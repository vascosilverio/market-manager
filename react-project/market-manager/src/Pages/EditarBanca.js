import React, { useState, useEffect } from 'react';
import { useParams, useHistory } from 'react-router-dom';
import { Form, Button } from 'react-bootstrap';
import axios from 'axios';
import { useAuth } from '../AuthContext';

function EditarBanca() {
  const { id } = useParams();
  const [nomeIdentificador, setNomeIdentificador] = useState('');
  const [categoria, setCategoria] = useState('');
  const [localizacaoX, setLocalizacaoX] = useState('');
  const [localizacaoY, setLocalizacaoY] = useState('');
  const [largura, setLargura] = useState('');
  const [comprimento, setComprimento] = useState('');
  const [estadoAtual, setEstadoAtual] = useState('');
  const [error, setError] = useState('');
  const history = useHistory();
  const { currentUser } = useAuth();

  useEffect(() => {
    const fetchBanca = async () => {
      try {
        const response = await axios.get(`https://localhost:7172/api/bancas/${id}`, {
          headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
        });
        const banca = response.data;
        setNomeIdentificador(banca.nomeIdentificadorBanca);
        setCategoria(banca.categoriaBanca.toString());
        setLocalizacaoX(banca.localizacaoX.toString());
        setLocalizacaoY(banca.localizacaoY.toString());
        setLargura(banca.largura.toString());
        setComprimento(banca.comprimento.toString());
        setEstadoAtual(banca.estadoAtualBanca.toString());
      } catch (error) {
        console.error('Error fetching banca:', error);
        setError('Failed to fetch banca details');
      }
    };

    fetchBanca();
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.put(`https://localhost:7172/api/bancas/${id}`, {
        nomeIdentificadorBanca: nomeIdentificador,
        categoriaBanca: parseInt(categoria),
        localizacaoX: parseInt(localizacaoX),
        localizacaoY: parseInt(localizacaoY),
        largura: parseFloat(largura),
        comprimento: parseFloat(comprimento),
        estadoAtualBanca: parseInt(estadoAtual)
      }, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      });
      history.push('/bancas');
    } catch (error) {
      console.error('Error updating banca:', error);
      setError('Failed to update banca');
    }
  };

  return (
    <div>
      <h2>Editar Banca</h2>
      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label>Nome Identificador</Form.Label>
          <Form.Control type="text" value={nomeIdentificador} onChange={(e) => setNomeIdentificador(e.target.value)} required />
        </Form.Group>
        <Form.Group>
          <Form.Label>Categoria</Form.Label>
          <Form.Control as="select" value={categoria} onChange={(e) => setCategoria(e.target.value)} required>
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
        <Form.Group>
          <Form.Label>Estado Atual</Form.Label>
          <Form.Control as="select" value={estadoAtual} onChange={(e) => setEstadoAtual(e.target.value)} required>
            <option value="0">Ocupada</option>
            <option value="1">Livre</option>
            <option value="2">Manutenção</option>
          </Form.Control>
        </Form.Group>
        {error && <p className="text-danger">{error}</p>}
        <Button type="submit">Atualizar Banca</Button>
      </Form>
    </div>
  );
}

export default EditarBanca;