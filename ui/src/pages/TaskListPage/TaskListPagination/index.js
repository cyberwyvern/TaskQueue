import Pagination from '@material-ui/lab/Pagination';
import React, { useContext } from 'react';
import StoreContext from '../../../stores/StoreContext';

function TaskListPagination({ ...props }) {
  const store = useContext(StoreContext).taskStore;

  const loadPage = (pageIndex) => {
    store.fetchPage(pageIndex, 15);
  };

  return (
    <Pagination {...props} onChange={(_event, pageNumber) => loadPage(pageNumber - 1)} />
  );
}

export default TaskListPagination;
