import { LoginUser, RegisterUser } from "../interfaces/User";
import http from "./httpRequest";

export class UserService {
  public static Login = (data: LoginUser) => {
    return http.post("user/Login", data);
  };

  public static Register(data: RegisterUser) {
    return http.post("user/Register", data);
  }
}
