import { SetPostData } from "../models/interfaces/Post";
import http from "./httpRequest";

export class PostService {
  public static GetAll = () => {
    return http.get("Post/GetAll");
  };

  public static Get = (id: number) => {
    return http.get(`Post/Get?id=${id}`);
  };

  public static Register = (postData: SetPostData) => {
    return http.post("Post/Register", postData);
  };
}
