import './CSS_Styles/App.css';
import { BrowserRouter as Router, Route, Switch} from "react-router-dom";
import Home from "./Pages/Home.js";
import Navbar from "./Components/Navbar";
import Footer from "./Components/Footer";

function App() {
  return (
    <div className="App">
      <Router>  
        <Navbar/>
        <Switch>
          <Route path="/" exact component={Home} />
        </Switch>
        <Footer/>  
      </Router>  
    </div>
  );
}

export default App;