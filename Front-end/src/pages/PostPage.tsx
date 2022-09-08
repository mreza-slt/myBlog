import { AuthRoute } from "../components/auth/AuthRoute";
import GetAllPost from "../components/post/GetAllPost";
import PostInfo from "../components/post/PostInfo";
import { NonAuthRoutes } from "../models/enums/AuthRoutes";

export default function PostPage(): JSX.Element {
  return (
    <>
      <AuthRoute path={NonAuthRoutes.posts} Component={GetAllPost} />
      <AuthRoute path={NonAuthRoutes.postInfo} Component={PostInfo} />
    </>
  );
}
