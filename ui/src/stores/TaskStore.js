import { HubConnectionBuilder } from '@microsoft/signalr';
import { action, makeObservable, observable, runInAction } from "mobx";
import config from '../config.json';

export default class TaskStore {
  _api;
  _hubConnection = null;
  _authenticationStore = null;

  items = [];
  pageIndex = 0;
  pageCount = 0;

  constructor(api, authenticationStore) {
    this._api = api;
    this._authenticationStore = authenticationStore;

    makeObservable(this, {
      items: observable,
      pageIndex: observable,
      pageCount: observable,
      fetchPage: action,
      taskChanged: action
    });
  }

  async createTask(task) {
    return await this._api.post('task', task);
  }

  async fetchPage(pageIndex, pageSize) {
    let response = await this._api.get('/task/page', {
      params: {
        pageIndex: pageIndex,
        pageSize: pageSize
      }
    });

    runInAction(() => {
      this.pageIndex = response.data.pageIndex;
      this.pageCount = response.data.pageCount;
      this.items = response.data.items;
    });
  }

  subscribeToExternalChanges() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(config.RESOURCE_API_URL + 'task-hub', {
        accessTokenFactory: () => this._authenticationStore.accessToken.access_token
      })
      .withAutomaticReconnect()
      .build();

    this._hubConnection.on('taskChanged', this.taskChanged);
    this._hubConnection.start();
  }

  unsubscribeFromExternalChanges() {
    this._hubConnection.stop();
    this._hubConnection = null;
  }

  taskChanged = (taskModel) => {
    console.log(taskModel.id);
    let index = this.items.map(i => i.id).indexOf(taskModel.id);
    if (index >= 0) {
      this.items[index] = taskModel;
    }
  }
}
