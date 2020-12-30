import { observer } from "mobx-react";
import { useContext } from "react";
import {
  BrowserRouter as Router,
  Route, Switch
} from "react-router-dom";
import styles from './App.module.css';
import Alert from "./components/Alert";
import GuardedRoute from './components/GuardedRoute';
import Header from './components/Header';
import Menu from './components/Menu';
import CreateTaskPage from "./pages/CreateTaskPage";
import LoginPage from './pages/LoginPage';
import TaskListPage from "./pages/TaskListPage";
import StoreContext from './stores/StoreContext';

function App() {
  const authenticationStore = useContext(StoreContext).authenticationStore;

  return (
    <Router>
      <div className={styles['root']}>
        <Header />
        <div className={styles['content']}>
          {authenticationStore.isAuthenticated && <Menu />}
          <div className={styles['page-content']}>
            <Switch>
              <GuardedRoute path='/' exact component={TaskListPage} altPath='/login' />
              <GuardedRoute path='/task-list' component={TaskListPage} altPath='/login' />
              <GuardedRoute path='/create-task' component={CreateTaskPage} altPath='/login' />

              <Route path="/login" component={LoginPage} />
            </Switch>
          </div>
        </div>
      </div>
      <Alert />
    </Router>
  );
}

export default observer(App);
