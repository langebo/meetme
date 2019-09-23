const makeActionCreator = (type, ...argNames) => {
  return (...args) => {
    const action = { type }
    argNames.forEach((arg, index) => {
      action[argNames[index]] = args[index]
    })
    return action
  }
}

export const setUser = makeActionCreator('SET_USER', 'user')
export const setUsers = makeActionCreator('SET_USERS', 'users')
export const setMeetings = makeActionCreator('SET_MEETINGS', 'meetings')
