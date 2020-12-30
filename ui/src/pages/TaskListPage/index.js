import {
  Paper, Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  withStyles
} from '@material-ui/core';
import { observer } from 'mobx-react';
import React, { useContext, useEffect } from 'react';
import StoreContext from '../../stores/StoreContext';
import TaskListItem from './TaskListItem';
import styles from './TaskListPage.module.css';
import TaskListPagination from './TaskListPagination';

const StyledTableCell = withStyles((theme) => ({
  head: {
    backgroundColor: theme.palette.secondary.main,
    color: theme.palette.common.white,
  },
}))(TableCell);

function TaskListPage() {
  const store = useContext(StoreContext).taskStore;

  useEffect(() => {
    store.subscribeToExternalChanges();
    store.fetchPage(0, 15);

    return () => {
      store.unsubscribeFromExternalChanges();
    };
  }, [store]);

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Task list
      </Typography>
      <TaskListPagination count={store.pageCount} className={styles['pagination-top']} />
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <StyledTableCell>#</StyledTableCell>
              <StyledTableCell>Title</StyledTableCell>
              <StyledTableCell>Created by</StyledTableCell>
              <StyledTableCell>Date created</StyledTableCell>
              <StyledTableCell colSpan={2}>Status</StyledTableCell>
              <StyledTableCell>Duration</StyledTableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {store.items.map(taskItem => <TaskListItem key={taskItem.id} taskItem={taskItem} />)}
          </TableBody>
        </Table>
      </TableContainer>
      <TaskListPagination count={store.pageCount} page={store.pageIndex + 1} className={styles['pagination-bottom']} />
    </div>
  );
}

export default observer(TaskListPage);
