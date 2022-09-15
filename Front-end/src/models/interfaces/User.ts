export interface LoginUser {
  userNameEmailPhone: string;
  password: string;
}

export interface SignupUser {
  title: string;
  name: string;
  surname: string;
  userName: string;
  email: string;
  phoneNumber: string;
  password: string;
  passwordConfirm: string;
}

export interface UserProfile {
  avatar: string;
  email: string | null;
  name: string;
  phoneNumber: string;
  surname: string | null;
  title: string | null;
  userName: string;
}
