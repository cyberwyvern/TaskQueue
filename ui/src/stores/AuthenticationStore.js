import { JSEncrypt } from 'jsencrypt';
import { computed, makeAutoObservable, runInAction } from "mobx";

export default class AuthenticationStore {
  _api;
  _rsa;
  accessToken;

  get isAuthenticated() {
    return !!this.accessToken;
  }

  constructor(api) {
    this._api = api;
    this._rsa = new JSEncrypt();
    this.accessToken = JSON.parse(localStorage.getItem("access_token"));

    makeAutoObservable(this, {
      _api: false,
      _rsa: false,
      isAuthenticated: computed
    });
  }

  async fetchRsaPublicKey() {
    let response = await this._api.get('security/public-key/rsa');
    runInAction(() => {
      this._rsa.setPublicKey(response.data.key);
    });
  }

  async loginOrRegister(username, password) {
    await this.fetchRsaPublicKey();
    let response = await this._api.post('auth/login-or-register', {
      username: username,
      password: this._rsa.encrypt(password)
    });

    runInAction(() => {
      this.accessToken = response.data;
      localStorage.setItem("access_token", JSON.stringify(this.accessToken));
    });
  }

  logout() {
    localStorage.removeItem("access_token");
    this.accessToken = null;
  }
}
