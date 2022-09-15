import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { logoutAsyncUser } from "../../features/user/userSlice";

export default function Logout() {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  useEffect(() => {
    dispatch(logoutAsyncUser());
    navigate("/");
  }, [dispatch, navigate]);

  return <></>;
}
