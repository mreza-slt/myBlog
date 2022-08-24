import { Route, Routes } from "react-router-dom";
import GetAllPost from "../components/post/GetAllPost";
import PostInfo from "../components/post/PostInfo";
import RegisterPost from "../components/post/RegisterPost";

export default function PostPage(): JSX.Element {
  return (
    <Routes>
      <Route path="/*" element={<GetAllPost />} />
      <Route path="/Register" element={<RegisterPost />} />
      <Route path=":id" element={<PostInfo />} />
    </Routes>
  );
}
