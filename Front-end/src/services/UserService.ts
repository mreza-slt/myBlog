import { LoginUser, SignupUser, UserProfile } from "../models/interfaces/User";
import http from "./httpRequest";

export class UserService {
  public static Login = (data: LoginUser) => {
    return http.post("user/Login", data);
  };

  public static Register(data: SignupUser) {
    return http.post("user/Register", data);
  }

  public static EditProfile(data: UserProfile) {
    return http.put("user/profile", data);
  }
}
