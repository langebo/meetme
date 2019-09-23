import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Stack } from 'office-ui-fabric-react'

import Meeting from './Meeting';
import { setMeetings } from '../store/actions'
import api from '../utils/api'

const MeetingStack = () => {
  const meetings = useSelector(state => state.meetings)
  const dispatch = useDispatch();

  useEffect(() => {
    api.get('/meetings')
      .then(response => dispatch(setMeetings(response.data)))
      .catch(error => console.error(error));
  }, [dispatch])

  const sectionStackTokens = {
    childrenGap: 12
  };

  return (
    <div>
      <Stack tokens={sectionStackTokens}>
        {meetings.map(meeting => <Meeting key={meeting.id} {...meeting} />)}
      </Stack>
    </div>
  );
};

export default MeetingStack;
