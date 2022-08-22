import axios from "axios";

axios.defaults.baseURL = "https://localhost:7154/";
axios.defaults.withCredentials = true;

const http = {
  get: axios.get,
  post: axios.post,
  put: axios.put,
  delete: axios.delete,
};

export default http;
