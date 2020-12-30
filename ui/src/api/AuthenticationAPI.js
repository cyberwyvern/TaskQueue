import axios from "axios";
import config from '../config.json';
import {
  setupErrorInerceptor,
  setupPendingResponsesInterceptor
} from './interceptors/Interceptors';

export default function AuthenticationAPI(rootStore) {
  const api = axios.create({
    baseURL: config.AUTHENTICATION_API_URL,
    headers: {
      'Content-type': 'application/json; charset=utf-8',
      'Accept': 'application/json; charset=utf-8'
    }
  });

  setupPendingResponsesInterceptor(api, rootStore);
  setupErrorInerceptor(api, rootStore);

  return api;
}
