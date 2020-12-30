import LinearProgress from '@material-ui/core/LinearProgress';
import { observer } from 'mobx-react';
import React, { useContext } from 'react';
import StoreContext from '../../stores/StoreContext';

function LoadingProgress() {
  const store = useContext(StoreContext).loadingProgressStore;

  return (
    <div>
      {store.isLoadingInProggress && <LinearProgress color="secondary" />}
    </div>
  );
}

export default observer(LoadingProgress);
