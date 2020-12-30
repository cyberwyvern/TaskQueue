import AppBar from '@material-ui/core/AppBar';
import Button from '@material-ui/core/Button';
import IconButton from '@material-ui/core/IconButton';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import MenuIcon from '@material-ui/icons/Menu';
import { observer } from 'mobx-react';
import React, { useContext } from 'react';
import { useHistory } from 'react-router-dom';
import StoreContext from '../../stores/StoreContext';
import LoadingProgress from "../LoadingProgress";
import styles from './Header.module.css';

function Header() {
  const stores = useContext(StoreContext);
  const history = useHistory();

  const handleLogout = () => {
    stores.authenticationStore.logout();
    history.push("/login");
  }

  return (
    <div>
      <AppBar position="fixed">
        <Toolbar>
          {
            stores.authenticationStore.isAuthenticated &&
            <IconButton
              edge="start"
              color="inherit"
              aria-label="menu"
              onClick={() => stores.menuStore.toggleMenu()}>
              <MenuIcon />
            </IconButton>
          }
          <Typography variant="h6" className={styles['title']}>
            Task Queue Manager
          </Typography>
          {
            stores.authenticationStore.isAuthenticated &&
            <Button color="inherit" onClick={handleLogout}>Logout</Button>
          }
        </Toolbar>
        <LoadingProgress />
      </AppBar>
      <Toolbar />
    </div>
  );
}

export default observer(Header);
