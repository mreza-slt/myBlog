import http from "./httpRequest";

export class UserService {
  public static Login = (data: any) => {
    return http.post("user/Login", data);
  };
}
