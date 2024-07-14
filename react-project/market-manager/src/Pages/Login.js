// src/Login.js
import React, { useState } from 'react';
import axios from 'axios';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState('');

    const handleLogin = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setMessage('');

        const url = `https://localhost:7172/api/SignInUser?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`;

        try {
            console.log('Iniciando requisição para:', url);

            const response = await axios.get(url);
            
            console.log('Resposta recebida:', response);

            // Tentar analisar a resposta como JSON, caso contrário tratar como texto simples
            try {
                const data = response.data;
                setMessage(`Login bem-sucedido: ${JSON.stringify(data)}`);
                localStorage.setItem('jwt', data);
                console.log('Dados recebidos:', data);

                // Verifique aqui se a resposta contém um token ou outra informação que indique um login bem-sucedido
                // e utilize-a conforme necessário (por exemplo, armazenar um token no localStorage)
                
            } catch (err) {
                setMessage(response.data);
                console.log('Resposta em texto:', response.data);
            }

        } catch (error) {
            console.error('Erro na requisição:', error);
            setError(error.response ? error.response.data : error.message);
        } finally {
            setLoading(false);
        }
        window.location.reload();
    };

    return (
        <div>
            <h2>Login</h2>
            <form onSubmit={handleLogin}>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                {error && <div style={{ color: 'red' }}>{error}</div>}
                {message && <div style={{ color: 'green' }}>{message}</div>}
                <button type="submit" disabled={loading}>
                    {loading ? 'A carregar...' : 'Login'}
                </button>
            </form>
        </div>
    );
};

export default Login;
