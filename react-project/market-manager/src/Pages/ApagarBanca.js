import React, { useState, useEffect } from 'react';
import { useParams, useHistory } from 'react-router-dom';
import { Card, Button } from 'react-bootstrap';
import axios from 'axios';

function ApagarBanca() {
  const { id } = useParams();
  const [banca, setBanca] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);
  const history = useHistory();

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

  const handleDelete = async () => {
    try {
      await axios.delete(`https://localhost:7172/api/bancas/${id}`, {
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
      });
      setSuccess(true);
      setTimeout(() => {
        history.push('/bancas');
      }, 2000);
    } catch (error) {
      console.error('Error deleting banca:', error);
      if (error.response.status === 409) {
        setError('Cannot delete banca with associated reservas');
      } else {
        setError('Failed to delete banca');
      }
    }
  };

  if (error) return <div className="error">{error}</div>;
  if (!banca) return <div>Loading...</div>;

  return (
    <div>
      <h2>Apagar Banca</h2>
      <Card>
        <Card.Body>
          <Card.Title>Tem certeza que deseja apagar esta banca?</Card.Title>
          <Card.Text>
            Banca ID: {banca.BancaId}<br />
            Nome Identificador: {banca.NomeIdentificadorBanca}<br />
            Categoria: {banca.CategoriaBanca}
          </Card.Text>
          {success ? (
            <p className="text-success">Banca apagada com sucesso!</p>
          ) : (
            <>
              <Button variant="danger" onClick={handleDelete} disabled={success}>Confirmar Exclusão</Button>
              <Button variant="secondary" onClick={() => history.goBack()} className="ms-2">Cancelar</Button>
            </>
          )}
        </Card.Body>
      </Card>
    </div>
  );
}

export default ApagarBanca;