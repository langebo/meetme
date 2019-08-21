import React from 'react';
import { StateProvider } from './utils/simply';
import Dashboard from './scenes/Dashboard';

const App = () => {
  const initialState = {
    user: { name: 'bobo', email: 'bobo@lala.com' },
    meetings: [
      { id: 423, title: 'sprint planning' },
      { id: 424, title: 'grooming' },
      { id: 425, title: 'review' },
    ],
    invitations: [
      { id: 654, title: 'team event' },
      { id: 871, title: 'meetup' },
      { id: 962, title: 'breakfast' },
      { id: 1007, title: 'team lead council' },
    ],
  };

  const reducer = (state, action) => {
    switch (action.type) {
      case 'setUser':
        return {
          ...state,
          user: action.user,
        };
      default:
        return state;
    }
  };

  return (
    <StateProvider initialState={initialState} reducer={reducer}>
      <Dashboard />
    </StateProvider>
  );
};

export default App;
