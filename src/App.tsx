// import React from 'react';
// import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
// import {QueryClient,QueryClientProvider} from "@tanstack/react-query"
// import { ThemeProvider } from 'styled-components';
// import { store } from "./actions/store"
// import { Provider } from 'react-redux';
// import { GlobalStyles,theme } from './styles';
// import * as S from "./pages"
// import { HomePageUser } from './pages/creativeTim/Homepage';
// // import { RequireAuth } from './collections/LoginForm/RequireAuth';
// // import { Welcome } from './collections/LoginForm/RequireAuth/Welcome';
// // import Layout from './actions/Layout';
// function App() {
//   return (
//     <Router>
//       <GlobalStyles/>
//         <Routes>
//           <Route path='/' element={<S.HomePage/>}/>
//           <Route path='/login' element={<S.LoginUpPage/>}/>
//           <Route path='/signup' element={<S.SignUpPage/>}/>
//           <Route path='/duser' element={<HomePageUser/>}/>

//           </Routes>
//           </Router>

//       /* <Router>
//       <Routes>
//         <Route path="/" element={<Layout/>}>
//           <Route index element ={<S.HomePage/>}/>
//           <Route path="/login" element={<S.LoginUpPage/>}/>
//           <Route path='/signup' element={<S.SignUpPage/>}/>

//           <Route element={<RequireAuth/>}>
//           <Route path="/welcome" element={<Welcome/>}/>
//           </Route>

//         </Route>
//       </Routes>
//       </Router>
//        */
//   );
// }
// export default App;

// // export default App;

// // import React from "react";
// // import "./App.css";
// // import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
// // import Main from "./pages/Main/Main";
// // import Login from "./pages/Login";
// // import Navbar from "./component/Navbar";
// // import CreatePost from "./pages/create-post/CreatePost";
// // import {QueryClient,QueryClientProvider} from "@tanstack/react-query"

// // const queryClient = new QueryClient()

// // function App() {
// //   return (
// //     <div className="App">
// //       <Router>
// //           <QueryClientProvider client={queryClient}>
// //         <Navbar />
// //         <Routes>
// //           <Route path="/" element={<Main />} />
// //           <Route path="/login" element={<Login />} />
// //           <Route path="/createpost" element={<CreatePost />} />
// //         </Routes>
// //         </QueryClientProvider>
// //       </Router>
// //     </div>
// //   );
// // }

// // export default App;

import React, { useState } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import { theme } from "./styles/";
import CssBaseline from "@mui/material/CssBaseline";
import Icon from "@mui/material/Icon";
import Box from "@mui/material/Box";
import Sidenav from "./pages/creativeTim/examples/Sidenav";
import routes from "./routes/routes";
import {
  useMaterialUIController,
  setMiniSidenav,
  setOpenConfigurator,
} from "./pages/creativeTim/context";
import { HomePageUser } from "./pages/creativeTim/Homepage";
import * as S from "./pages";
import { ProtectedRoute } from "./routes/ProtectedRoute/ProtectedRoute";

interface RouteType {
  collapse?: boolean;
  route?: string;
  component?: React.ReactNode;
  key: string;
}

// const App: React.FC = () => {
//   const [controller, dispatch] = useMaterialUIController();
//   const {
//     miniSidenav,
//     direction,
//     layout,
//     openConfigurator,
//     sidenavColor,
//     transparentSidenav,
//     whiteSidenav,
//     darkMode,
//   } = controller;
//   const [onMouseEnter, setOnMouseEnter] = useState<boolean>(false);

//   // const handleOnMouseEnter = () => {
//   //   if (miniSidenav && !onMouseEnter) {
//   //     setMiniSidenav(dispatch, false);
//   //     setOnMouseEnter(true);
//   //   }
//   // };

//   // const handleOnMouseLeave = () => {
//   //   if (onMouseEnter) {
//   //     setMiniSidenav(dispatch, true);
//   //     setOnMouseEnter(false);
//   //   }
//   // };

//   const handleConfiguratorOpen = () =>
//     setOpenConfigurator(dispatch, !openConfigurator);

//   const getRoutes = (allRoutes: RouteType[]): React.ReactNode =>
//     allRoutes.map((route) => {
//       return (
//         <Route path={route.route} element={route.component} key={route.key} />
//       );
//     });

//   const configsButton = (
//     <Box
//       display="flex"
//       justifyContent="center"
//       alignItems="center"
//       width="3.25rem"
//       height="3.25rem"
//       bgcolor="white"
//       boxShadow={1}
//       borderRadius="50%"
//       position="fixed"
//       right="2rem"
//       bottom="2rem"
//       zIndex={99}
//       color="text.primary"
//       sx={{ cursor: "pointer" }}
//       onClick={handleConfiguratorOpen}
//     >
//       <Icon fontSize="small" color="inherit">
//         settings
//       </Icon>
//     </Box>
//   );

//   return (
//     <ThemeProvider theme={theme}>
//       <CssBaseline />
//       {/* {layout === "dashboard" && (
//         <>
//           <Sidenav
//             color={sidenavColor}
//             brandName="BankAcc Management"
//             routes={routes}
//             // onMouseEnter={handleOnMouseEnter}
//             // onMouseLeave={handleOnMouseLeave}
//           />
//           {configsButton}
//         </>
//       )} */}
//       <Routes>
//         {getRoutes(routes)}
//         <Route path="/duser" element={<HomePageUser />} />
//         <Route path="/login" element={<S.LoginUpPage />} />
//         <Route path="/signup" element={<S.SignUpPage />} />

//         <Route path="*" element={<Navigate to="/login" />} />
//       </Routes>
//     </ThemeProvider>
//   );
// };
const App = () =>{

  return (
    <>
      {/* <Routes>
        
        <Route path="/login" element={<S.LoginUpPage />} />
        <Route path="/signup" element={<S.SignUpPage />} />

        <Route index element={<S.LoginUpPage />}>
          <Route path="landing" element={<S.LoginUpPage />} />
          <Route element={<ProtectedRoute
          
          isAllowed={true}
          //if the user is logged in
          />} >
        <Route path="home" element={<S.LoginUpPage/>}/>
        <Route path="dashboard" element={<S.LoginUpPage/>}/>  
            </Route>
            <Route path="admin" element={
              <ProtectedRoute 
              redirectedPath="/home"
              isAllowed={
               false
               //important admin permission
              }
              >

                <Route path="users" element={<S.LoginUpPage/>}/>
                <Route path="user/info" element={<S.LoginUpPage/>}/>
              </ProtectedRoute>
            }>
              
            </Route>
        </Route>
        
      </Routes> */}
    </>
  );
}

export default App;
