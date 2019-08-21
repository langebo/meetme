import React from 'react';
import { useStateContext } from '../utils/simply';
import Invitation from './Invitation';

const InvitationList = () => {
  const [state] = useStateContext();

  return (
    <div>
      <h2>My invites</h2>
      <ul>
        {state.invitations.map(invite => (
          <li key={invite.id}>
            <Invitation invite={invite} />
          </li>
        ))}
      </ul>
    </div>
  );
};

export default InvitationList;
