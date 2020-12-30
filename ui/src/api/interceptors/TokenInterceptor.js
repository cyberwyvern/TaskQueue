export default function setupTokenInterceptor(axiosInstance, rootStore) {
  const onRequestFulfilled = (config) => {
    if (rootStore.authenticationStore.isAuthenticated) {
      let token = rootStore.authenticationStore.accessToken.access_token;
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  }

  axiosInstance.interceptors.request.use(onRequestFulfilled)
}
