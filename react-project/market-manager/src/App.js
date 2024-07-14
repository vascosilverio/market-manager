import './CSS_Styles/App.css';
import { BrowserRouter as Router, Route, Switch} from "react-router-dom";
import Home from "./Pages/Home.js";
import Navbar from "./Components/Navbar";
import Footer from "./Components/Footer";
import Bancas from "./Pages/Bancas";
import BancaEliminar from "./Pages/BancaEliminar";
import BancaEditar from "./Pages/BancaEditar";
import BancaCriar from './Pages/BancaCriar.js';
import Reservas from "./Pages/Reservas";
import ReservaEliminar from "./Pages/ReservaEliminar";
import Login from "./Pages/Login";

function App() {
  return (
    <div className="App">
      <Router>  
        <Navbar/>
        <Switch>
          <Route path="/" exact component={Home} />
          <Route path="/criar_banca" exact component={BancaCriar} />
          <Route path="/bancas" exact component={Bancas} />
          <Route path="/eliminar_banca/:bancaId" exact component={BancaEliminar} />
          <Route path="/editar_banca/:bancaId" exact component={BancaEditar} />
          <Route path="/reservas" exact component={Reservas} />
          <Route path="/eliminar_reserva/:reservaId" exact component={ReservaEliminar} />
          <Route path="/login" exact component={Login} />
        </Switch>
        <Footer/>  
      </Router>  
    </div>
  );
}

export default App;