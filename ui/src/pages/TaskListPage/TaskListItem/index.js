import { CircularProgress, TableCell, TableRow, withStyles } from '@material-ui/core';
import { green, red } from '@material-ui/core/colors';
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline';
import ErrorOutlineIcon from '@material-ui/icons/ErrorOutline';
import moment from 'moment';
import React from 'react';
import styles from './TaskListItem.module.css';

const StyledTableRow = withStyles((theme) => ({
  root: {
    '&:nth-of-type(odd)': {
      backgroundColor: theme.palette.action.hover,
    },
  },
}))(TableRow);

function TaskListItem({ taskItem }) {
  return (
    <StyledTableRow key={taskItem.id}>
      <TableCell>{taskItem.sequenceNumber}</TableCell>
      <TableCell component="th" scope="row">{taskItem.title}</TableCell>
      <TableCell>{taskItem.createdBy}</TableCell>
      <TableCell>{moment(new Date(taskItem.createdDate)).format("DD-MM-YYYY hh:mm:ss")}</TableCell>
      <TableCell>{taskItem.status}</TableCell>
      <TableCell>
        {
          taskItem.status === "Created" &&
          <CircularProgress color="secondary" size={25} />
        }
        {
          taskItem.status === "Completed" &&
          <CheckCircleOutlineIcon style={{ color: green[500] }} className={styles['status-icon']} />
        }
        {
          taskItem.status === "Failed" &&
          <ErrorOutlineIcon color="success.main" style={{ color: red[500] }} className={styles['status-icon']} />
        }
      </TableCell>
      <TableCell>{taskItem.durationMs && moment.utc(taskItem.durationMs).format("HH:mm:ss")}</TableCell>
    </StyledTableRow>
  );
}

export default TaskListItem;
