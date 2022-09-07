import { useState } from "react";
import { useSelector } from "react-redux";
import DialogComponent from "../../common/Dialog";
import EditProfileForm from "../../components/user/EditProfileForm";
import { RootState } from "../../features/store";
import { UserProfile } from "../../models/interfaces/User";

export default function ProfilePage() {
  const [open, setOpen] = useState<boolean>(false);

  const user: UserProfile =
    useSelector((state: RootState) => state.user.user) ||
    JSON.parse(localStorage.getItem("stateUser")!);

  return (
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
        <div className="flex justify-end">
          <button
            onClick={() => setOpen(true)}
            className="w-full items-center transition-all duration-300 text-indigo-700 hover:text-white border border-indigo-700 hover:bg-indigo-800 focus:ring-4 focus:outline-none focus:ring-indigo-300 font-medium rounded-lg text-sm py-2.5 text-center mb-2 dark:border-indigo-500 dark:text-indigo-500 dark:hover:text-white dark:hover:bg-indigo-600 dark:focus:ring-indigo-900"
          >
            ویرایش
          </button>
        </div>
      </div>
      <DialogComponent setOpen={setOpen} open={open}>
        <EditProfileForm setOpen={setOpen} user={user} />
      </DialogComponent>
    </div>
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
