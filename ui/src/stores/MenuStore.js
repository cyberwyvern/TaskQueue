import { makeAutoObservable } from "mobx";

export default class MenuStore {
  menuOpened = false;

  constructor() {
    makeAutoObservable(this);
  }

  toggleMenu = () => this.menuOpened = !this.menuOpened;
}
