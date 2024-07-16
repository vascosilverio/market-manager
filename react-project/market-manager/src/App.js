import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import Navbar from "./Pages/Navbar";
import Bancas from "./Pages/Bancas";
import Reservas from "./Pages/Reservas";
import Home from "./Pages/Home";
import Login from "./Pages/Login";
import Register from "./Pages/Register";
import CriarReserva from "./Pages/CriarReserva";
import EditarReserva from "./Pages/EditarReserva";
import ApagarReserva from "./Pages/ApagarReserva";
import DetalhesReserva from "./Pages/DetalhesReserva";
import CriarBanca from "./Pages/CriarBanca";
import EditarBanca from "./Pages/EditarBanca";
import ApagarBanca from "./Pages/ApagarBanca";
import DetalhesBanca from "./Pages/DetalhesBanca";
import 'bootstrap/dist/css/bootstrap.min.css';
import { AuthProvider } from "./AuthContext";
import './CSS_Styles/styles.css';  // Unified CSS file

function App() {
  return (
    <div className="App">
      <Router>
        <AuthProvider>
          <Navbar />
          <Switch>
            <Route path="/" exact component={Home} />
            <Route path="/home" exact component={Home} />
            <Route path="/bancas" exact component={Bancas} />
            <Route path="/reservas" exact component={Reservas} />
            <Route path="/login" exact component={Login} />
            <Route path="/register" exact component={Register} />
            <Route path="/criar_reserva" exact component={CriarReserva} />
            <Route path="/editar_reserva/:id" exact component={EditarReserva} />
            <Route path="/apagar_reserva/:id" exact component={ApagarReserva} />
            <Route path="/detalhes_reserva/:id" exact component={DetalhesReserva} />
            <Route path="/criar_banca" exact component={CriarBanca} />
            <Route path="/editar_banca/:id" exact component={EditarBanca} />
            <Route path="/apagar_banca/:id" exact component={ApagarBanca} />
            <Route path="/detalhes_banca/:id" exact component={DetalhesBanca} />
          </Switch>
        </AuthProvider>
      </Router>
    </div>
  );
}

export default App;