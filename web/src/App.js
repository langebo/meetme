import React from 'react';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { Security, ImplicitCallback } from '@okta/okta-react';
import Dashboard from './scenes/Dashboard'
import Login from './scenes/Login'

const config = {
  issuer: 'https://dev-594008.okta.com/oauth2/default',
  redirect_uri: window.location.origin + '/implicit/callback',
  client_id: '0oa1561c1j3NHq1fh357'
}

function App() {
  return (
    <Router>
        <Security issuer={config.issuer}
                  client_id={config.client_id}
                  redirect_uri={config.redirect_uri}>
          <Route path='/' exact={true} component={Login}/>
          <Route path='/dashboard' exact={true} component={Dashboard}/>
          <Route path='/implicit/callback' component={ImplicitCallback}/>
        </Security>
      </Router>
  );
}

export default App;
