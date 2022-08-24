import http from "./httpRequest";

export class PostService {
  public static GetAll = () => {
    return http.get("Post/GetAll");
  };
}
