export interface LoginUser {
  userNameEmailPhone: string;
  password: string;
}

export interface RegisterUser {
  title: string;
  name: string;
  surname: string;
  userName: string;
  email: string;
  phoneNumber: string;
  password: string;
  passwordConfirm: string;
}
