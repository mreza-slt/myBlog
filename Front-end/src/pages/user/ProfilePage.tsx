import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import Button from "../../common/Button";
import DialogComponent from "../../common/Dialog";
import EditProfileForm from "../../components/user/EditProfileForm";
import { RootState } from "../../features/store";
import { UserProfile } from "../../models/interfaces/User";

export default function ProfilePage() {
  const [open, setOpen] = useState<boolean>(false);

  const { token } = useSelector((state: RootState) => state.user);
  const user: UserProfile | null = useSelector(
    (state: RootState) => state.user.user
  );

  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate("/user/login?redirect=user/profile");
    }
  }, [navigate, token]);

  return (
    user && (
      <div className="bg-slate-100">
        <div>
          <div className="flex justify-start items-center py-3">
            <span className="ml-1 h-12 w-12 rounded-full overflow-hidden bg-gray-100">
              <img
                src={user.avatar}
                alt="تصویر کاربر"
                className="h-full w-full text-gray-300"
              />
            </span>
          </div>
          <UserInfo label="نام کاربری" value={user.userName} />
          <UserInfo label="نام" value={user.title + " " + user.name} />
          <UserInfo label="نام خانوادگی" value={user.surname} />
          <UserInfo label="ایمیل" value={user.email} />
          <UserInfo label="شماره تلفن" value={user.phoneNumber} />
        </div>
        <div className="pt-5">
          <div className="flex justify-end" onClick={() => setOpen(true)}>
            <Button>ویرایش</Button>
          </div>
        </div>
        <DialogComponent setOpen={setOpen} open={open}>
          <EditProfileForm setOpen={setOpen} user={user} />
        </DialogComponent>
      </div>
    )
  );
}

type MiniUserInfo = {
  label: string;
  value: string | null;
};

const UserInfo = ({ label, value }: MiniUserInfo) => {
  return (
    <div className="flex justify-start items-center py-3 sm:border-t sm:border-gray-200">
      <h1 className="block text-base font-medium text-gray-700">
        {label + " : "}
      </h1>
      <div className="mt-1 mr-1">
        <h2 className="bg-slate-100 w-full py-1">{value}</h2>
      </div>
    </div>
  );
};
