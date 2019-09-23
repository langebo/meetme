const initialState = {
  user: {},
  users: [],
  meetings: []
}

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case 'SET_USER':
      return { ...state, user: action.user };
    case 'SET_USERS':
      return { ...state, users: action.users };
    case 'SET_MEETINGS':
      return { ...state, meetings: action.meetings };
    default:
      return state;
  }
}

export default reducer;
