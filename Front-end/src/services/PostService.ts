import { RegisterPostForm } from "../interfaces/Post";
import http from "./httpRequest";

export class PostService {
  public static GetAll = () => {
    return http.get("Post/GetAll");
  };

  public static Register = (postData: RegisterPostForm, image: File) => {
    let formData = new FormData();
    const config = {
      headers: { "content-type": "multipart/form-data" },
    };
    formData.append("image", image);
    formData.append("subjectId", postData.subjectId);
    formData.append("childSubjectId", postData.childSubjectId);
    formData.append("text", postData.text);
    formData.append("title", postData.title);
    return http.post("Post/Register", formData, config);
  };
}
