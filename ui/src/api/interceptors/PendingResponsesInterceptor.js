export default function setupPendingResponsesInterceptor(axiosInstance, rootStore) {
  const onRequestFulfilled = (config) => {
    rootStore.loadingProgressStore.incrementPendingRequestsNumber();
    return config;
  }

  const onResponseFulfilled = (response) => {
    rootStore.loadingProgressStore.decrementPendingRequestsNumber();
    return response;
  }

  const onResponseRejected = (error) => {
    rootStore.loadingProgressStore.decrementPendingRequestsNumber();
    return Promise.reject(error);
  }

  axiosInstance.interceptors.request.use(onRequestFulfilled)
  axiosInstance.interceptors.response.use(onResponseFulfilled, onResponseRejected);
}
