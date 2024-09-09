import React from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import {QueryClient,QueryClientProvider} from "@tanstack/react-query"
import { ThemeProvider } from 'styled-components';
import { GlobalStyles,theme } from './styles';
import * as S from "./pages"
function App() {
  return (
    <div className="App">
      <GlobalStyles/>
      <Router>
        <Routes>
          <Route path='/' element={<S.HomePage/>}/>
          <Route path='/login' element={<S.LoginUpPage/>}/>
          <Route path='/signup' element={<S.SignUpPage/>}/>
        </Routes>
      </Router>
      
    </div>
  );
}
export default App;

// export default App;

// import React from "react";
// import "./App.css";
// import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
// import Main from "./pages/Main/Main";
// import Login from "./pages/Login";
// import Navbar from "./component/Navbar";
// import CreatePost from "./pages/create-post/CreatePost";
// import {QueryClient,QueryClientProvider} from "@tanstack/react-query"

// const queryClient = new QueryClient()

// function App() {
//   return (
//     <div className="App">
//       <Router>
//           <QueryClientProvider client={queryClient}>
//         <Navbar />
//         <Routes>
//           <Route path="/" element={<Main />} />
//           <Route path="/login" element={<Login />} />
//           <Route path="/createpost" element={<CreatePost />} />
//         </Routes>
//         </QueryClientProvider>
//       </Router>
//     </div>
//   );
// }

// export default App;
