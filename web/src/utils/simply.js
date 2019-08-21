import React, { createContext, useContext, useReducer } from 'react';

export const StateContext = createContext();

export const StateProvider = ({ reducer, initialState, children }) => (
  <StateContext.Provider value={useReducer(reducer, initialState)}>
    {children}
  </StateContext.Provider>
);

export const useStateContext = () => useContext(StateContext);

// this is copied from https://github.com/lukashala/react-simply/blob/master/tools/state/src/index.js
// because the npm package runs into babel es6 transpilation error when not configuring explicitly in
// webpack config. since create-react-app was used ejecting this would be crazy at this point, I decided
// to copy the implementation, while issue https://github.com/lukashala/react-simply/issues/2 stays
// unsresolved
