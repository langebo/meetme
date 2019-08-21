import React from 'react';
import { useStateContext } from '../utils/simply';
import Meeting from './Meeting';

const MeetingList = () => {
  const [state] = useStateContext();

  return (
    <div>
      <h2>My meetings</h2>
      <ul>
        {state.meetings.map(meeting => (
          <li key={meeting.id}>
            <Meeting meeting={meeting} />
          </li>
        ))}
      </ul>
    </div>
  );
};

export default MeetingList;
