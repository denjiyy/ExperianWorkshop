import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import axios from 'axios';
import { Provider } from 'react-redux';
import { store } from './actions/store';
import { AuthProvider } from './axios_auth/context/AuthProvider';
import { MaterialUIControllerProvider } from './pages/creativeTim/context';
import { BrowserRouter as Router } from 'react-router-dom';
const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);


root.render(
  <React.StrictMode>
    <AuthProvider>
      <MaterialUIControllerProvider>
    <Provider store={store}>
      <Router>
    <App />
    </Router>
  </Provider>
  </MaterialUIControllerProvider>
  </AuthProvider>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
