import React from 'react';
import MeetingList from '../components/MeetingList';
import InvitationList from '../components/InvitationList';
import User from '../components/User';

const Dashboard = () => {
  return (
    <div>
      <h1>Dashboard</h1>
      <div>
        <User />
      </div>
      <div>
        <MeetingList />
      </div>
      <div>
        <InvitationList />
      </div>
    </div>
  );
};

export default Dashboard;
