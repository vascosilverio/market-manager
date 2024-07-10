import './CSS_Styles/App.css';
import { BrowserRouter as Router, Route, Switch} from "react-router-dom";
import Home from "./Pages/Home.js";
import Navbar from "./Components/Navbar";
import Footer from "./Components/Footer";
import Bancas from "./Pages/Bancas";

function App() {
  return (
    <div className="App">
      <Router>  
        <Navbar/>
        <Switch>
          <Route path="/" exact component={Home} />
          <Route path="/bancas" exact component={Bancas} />
        </Switch>
        <Footer/>  
      </Router>  
    </div>
  );
}

export default App;