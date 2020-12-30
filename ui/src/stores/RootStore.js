import AuthenticationAPI from "../api/AuthenticationAPI";
import ResourceAPI from "../api/ResourceAPI";
import AlertStore from './AlertStore';
import AuthenticationStore from "./AuthenticationStore";
import LoadingProgressStore from './LoadingProgressStore';
import MenuStore from './MenuStore';
import TaskStore from "./TaskStore";

class RootStore {
  constructor() {
    this.authAPI = new AuthenticationAPI(this);
    this.resourceAPI = new ResourceAPI(this);
    this.authenticationStore = new AuthenticationStore(this.authAPI);
    this.taskStore = new TaskStore(this.resourceAPI, this.authenticationStore);
    this.loadingProgressStore = new LoadingProgressStore();
    this.alertStore = new AlertStore();
    this.menuStore = new MenuStore();
  }
}

export default new RootStore();
