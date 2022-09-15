import { Route, Routes } from "react-router-dom";
import GetAllPost from "../components/post/GetAllPost";
import PostInfo from "../components/post/PostInfo";

export default function PostPage(): JSX.Element {
  return (
    <Routes>
      <Route path="/*" element={<GetAllPost />} />
      <Route path=":id" element={<PostInfo />} />
    </Routes>
  );
}
