import React from 'react';
import MeetingList from '../components/MeetingList';
import User from '../components/User';
import styled from 'styled-components'

const Dashboard = () => {
  return (
    <div>
      <h1>Dashboard</h1>
      <div>
        <MeetingList />
      </div>
    </div>
  );
};

export default Dashboard;
