export enum AuthRoutes {
  profile = "profile",
  logout = "/user/logout",
  // post
  registerPost = "/post/register",
}

export enum NonAuthRoutes {
  //post
  posts = "/*",
  postInfo = ":id",
  //user
  login = "/user/login",
  signup = "/signup",
}
