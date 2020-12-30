import { observer } from 'mobx-react';
import React, { useContext } from 'react';
import { Redirect, Route } from "react-router-dom";
import StoreContext from '../../stores/StoreContext';

function GuardedRoute({ component: Component, altPath, ...rest }) {
  const store = useContext(StoreContext).authenticationStore;

  return (
    <Route {...rest} render={(props) => (
      store.isAuthenticated
        ? <Component {...props} />
        : <Redirect to={altPath} />
    )} />
  );
}

export default observer(GuardedRoute);
