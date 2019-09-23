import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Persona } from 'office-ui-fabric-react'
import { setUser } from '../store/actions'
import api from '../utils/api'
import generateInitials from '../utils/initials'

const User = () => {
  const user = useSelector(state => state.user)
  const dispatch = useDispatch();

  useEffect(() => {
    api.get('/users/me')
      .then(response => {
        console.log(response.data);
        return dispatch(setUser(response.data));
      })
      .catch(error => console.error(error));
  }, [dispatch]);

  const persona = {
    text: user.name,
    secondaryText: user.email,
    imageInitials: generateInitials(user.name),
    initialsColor: user.id ? `#${user.id.substring(0, 6)}` : '#FFFFFF',
    dir: 'rtl',
    styles: {
      primaryText: { textAlign: 'right' },
      secondaryText: { textAlign: 'right' }
    }
  }

  return <Persona {...persona} />
};

export default User;
