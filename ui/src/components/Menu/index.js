import { List, ListItem, ListItemIcon, ListItemText, Paper } from '@material-ui/core';
import AssignmentIcon from '@material-ui/icons/Assignment';
import NoteAddIcon from '@material-ui/icons/NoteAdd';
import { observer } from 'mobx-react';
import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import StoreContext from '../../stores/StoreContext';
import styles from './Menu.module.css';

function Menu() {
  const store = useContext(StoreContext).menuStore;

  const getRootClasses = () => {
    return store.menuOpened
      ? styles['root']
      : styles['root'] + ' ' + styles['root-closed']
  }

  return (
    <div className={getRootClasses()}>
      <Paper variant="outlined" square className={styles['content']}>
        <List component="nav">
          <ListItem button component={Link} to='/task-list'>
            <ListItemIcon>
              <AssignmentIcon />
            </ListItemIcon>
            <ListItemText primary="Task list" />
          </ListItem>
          <ListItem button component={Link} to='/create-task'>
            <ListItemIcon>
              <NoteAddIcon />
            </ListItemIcon>
            <ListItemText primary="Create task" />
          </ListItem>
        </List>
      </Paper>
    </div>
  );
}

export default observer(Menu);
