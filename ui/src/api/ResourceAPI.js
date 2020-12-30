import axios from "axios";
import config from '../config.json';
import {
  setupErrorInerceptor,
  setupPendingResponsesInterceptor,
  setupTokenInterceptor
} from './interceptors/Interceptors';

export default function ResourceAPI(rootStore) {
  const api = axios.create({
    baseURL: config.RESOURCE_API_URL,
    headers: {
      'Content-type': 'application/json; charset=utf-8',
      'Accept': 'application/json; charset=utf-8'
    }
  });

  setupPendingResponsesInterceptor(api, rootStore);
  setupErrorInerceptor(api, rootStore);
  setupTokenInterceptor(api, rootStore);

  return api;
}
