import React from 'react';
import { useStateContext } from '../utils/simply';

const User = () => {
  const [state] = useStateContext();

  return (
    <h3>
      {state.user.name} {state.user.email}
    </h3>
  );
};

export default User;
