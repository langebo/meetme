import React from 'react';
import { HashRouter as Router, Route } from 'react-router-dom'
import styled from 'styled-components';
import { initializeIcons } from '@uifabric/icons';

import Dashboard from './scenes/Dashboard';
import User from './components/User'

initializeIcons();

const App = () => {
  return (
    <Grid>
      <Header>meet<HeaderAccent>me</HeaderAccent></Header>
      <Account>
        <User />
      </Account>
      <Main>
        <Router>
          <Route path='/' component={Dashboard} />
        </Router>
      </Main>
    </Grid>
  );
};

export default App;

const Grid = styled.div`
  margin-top: 24px;
  height: 100vh;
  display: grid;
  grid-template-columns: 1fr 6fr 2fr 1fr;
  grid-template-rows: 48px 1fr;
  grid-template-areas:
    'left head account right'
    'left main main right';
`
const Header = styled.span`
  grid-area: head;
  font-size: 42px;
  align-self: center;
  font-weight: 200;
`

const HeaderAccent = styled.span`
  color: #b71540;
  font-weight: 600;
`

const Account = styled.div`
  grid-area: account;
`

const Main = styled.div`
  grid-area: main;
`
