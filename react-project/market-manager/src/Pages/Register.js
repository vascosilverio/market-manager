import React, { useState } from 'react';
import axios from 'axios';
import { Form, Button, Alert } from 'react-bootstrap';
import { useAuth } from '../AuthContext';
import { useHistory } from 'react-router-dom';

const Register = () => {

    
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        nomeCompleto: '',
        dataNascimento: '',
        telemovel: '',
        morada: '',
        codigoPostal: '',
        localidade: '',
        cc: '',
        nif: ''
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const history = useHistory();
    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

   

    const handleSubmit = async (e) => {
      e.preventDefault();
      setLoading(true);
      setError('');
  
      try {
        await axios.post('https://localhost:7172/api/registo', formData);
  
        const response = await axios.post('https://localhost:7172/api/login', {
          email: formData.email,
          password: formData.password
        });
  
        const data = response.data;
        login(data.token, data.userRole, data.userId);
        history.push('/');
      } catch (error) {
        console.error('Erro na requisição:', error);
        setError(error.response ? error.response.data : error.message);
      } finally {
        setLoading(false);
      }
    };
    

    return (
        <div>
            <h2>Registo</h2>
            <Form onSubmit={handleSubmit}>
                <Form.Group controlId="email">
                    <Form.Label>Email:</Form.Label>
                    <Form.Control
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="password">
                    <Form.Label>Password:</Form.Label>
                    <Form.Control
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="nomeCompleto">
                    <Form.Label>Nome Completo:</Form.Label>
                    <Form.Control
                        type="text"
                        name="nomeCompleto"
                        value={formData.nomeCompleto}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="dataNascimento">
                    <Form.Label>Data de Nascimento:</Form.Label>
                    <Form.Control
                        type="date"
                        name="dataNascimento"
                        value={formData.dataNascimento}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="telemovel">
                    <Form.Label>Telemóvel:</Form.Label><Form.Control
                        type="tel"
                        name="telemovel"
                        value={formData.telemovel}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="morada">
                    <Form.Label>Morada:</Form.Label>
                    <Form.Control
                        type="text"
                        name="morada"
                        value={formData.morada}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="codigoPostal">
                    <Form.Label>Código Postal:</Form.Label>
                    <Form.Control
                        type="text"
                        name="codigoPostal"
                        value={formData.codigoPostal}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="localidade">
                    <Form.Label>Localidade:</Form.Label>
                    <Form.Control
                        type="text"
                        name="localidade"
                        value={formData.localidade}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="cc">
                    <Form.Label>Cartão de Cidadão:</Form.Label>
                    <Form.Control
                        type="text"
                        name="cc"
                        value={formData.cc}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                <Form.Group controlId="nif">
                    <Form.Label>NIF:</Form.Label>
                    <Form.Control
                        type="text"
                        name="nif"
                        value={formData.nif}
                        onChange={handleChange}
                        required
                    />
                </Form.Group>
                {error && <Alert variant="danger">{error}</Alert>}
                <Button type="submit" disabled={loading}>
                    {loading ? 'Carregando...' : 'Registar'}
                </Button>
            </Form>
        </div>
    );
};
export default Register;