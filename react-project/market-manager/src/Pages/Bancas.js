import React, { useEffect, useState } from 'react';
import Axios from "axios";
import "../CSS_Styles/Bancas.css";

function Bancas() {
  //varivel de estato que armazena o conteúdo do excel com os posts
  //useState é um hook e posts é a variável de estado atual, sendo que set_post é a função que a atualiza
  const [posts, set_post] = useState([]);
  const [search, setSearch] = useState("");

  //transforma o conteúdo do excel num objeto e carrega para o site (com recurso a API)
    useEffect(() =>{
    Axios.get('https://sheetdb.io/api/v1/sx7b7l1ren6nm?sheet=posts').then((response) => {
      set_post(response.data); //o estado "posts" é atualizado com os dados da resposta da solicitação HTTP
    });
  },[]);
  //este useEffect só vai ser executado uma vez, pois
  //está inserido no "final" um array vazio
  
  //para uso na funcionalidade de pesquisa
  const filteredPosts = posts.filter(post =>
    post.Nome.toLowerCase().includes(search.toLowerCase()) ||
    post.Descricao.toLowerCase().includes(search.toLowerCase())
  );

  return (
    //funcionalidade de pesquisa
    <div className='Bancas'>
      <h1 className=''></h1>
      <input
        type="text"
        placeholder="Pesquisar..."
        onChange={e => setSearch(e.target.value)}
      />
      {filteredPosts.length > 0 ? filteredPosts.map((post, index) => (
        //dar display do conteúdo do excel no ecrã
        <div key={index}>
          <p>Publicação de {post.Nome}</p>
          <img className='FotoDestino' src={post.Imagem} />
          <p className="descricao">Descrição:</p>
          <p>{post.Descricao}</p>
        </div>
      )) : <p>Loading...</p>}
    </div>
  );
}

//<div key={index}>
//Esta linha começa a definição de um elemento div para cada post. 
//A propriedade key é usada para dar a cada div uma chave única. 
//É necessária ao criar listas de elementos em React.
// posts.map((post, index) , post vai ser o conteúdo da linha do excel
// por isso é que fazemos post.Nome e etc
// <p>Loading...</p> é para caso não haja conteúdo (ou ainda esteja a ser carregado) ou aconteça alguma
// coisa vá impedir o display do conteúdo, caso isto aconteça, é mostrado "Loading..."

export default Bancas;