import { PostDataForm } from "../models/interfaces/Post";
import http from "./httpRequest";

export class PostService {
  public static GetAll = () => {
    return http.get("Post/GetAll");
  };

  public static Get = (id: number) => {
    return http.get(`Post/Get?id=${id}`);
  };

  public static Register = (postData: PostDataForm) => {
    let formData = new FormData();
    const config = {
      headers: { "content-type": "multipart/form-data" },
    };
    formData.append("image", postData.image);
    formData.append("subjectId", postData.subjectId);
    formData.append("childSubjectId", postData.childSubjectId);
    formData.append("text", postData.text);
    formData.append("title", postData.title);
    return http.post("Post/Register", formData, config);
  };
}
