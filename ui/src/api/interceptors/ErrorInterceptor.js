export default function setupErrorInerceptor(axiosInstance, rootStore) {
  const onRejected = (error) => {
    if (error.response.status === 401) {
      rootStore.alertStore.showWarning("session expired, please relogin");
      rootStore.authenticationStore.logout();
      return Promise.reject(error);
    } else {
      rootStore.alertStore.showError("error");
      return Promise.reject(error);
    }
  }

  axiosInstance.interceptors.response.use(null, onRejected);
}
