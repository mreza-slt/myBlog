import { faAngleDown, faAngleUp } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FormikProps, useFormik } from "formik";
import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import * as Yup from "yup";
import Button from "../../common/Button";
import Error from "../../common/Error";
import Input from "../../common/Input";
import Loading from "../../common/Loading";
import { RootState } from "../../features/store";
import { editProfileAsyncUser } from "../../features/user/userSlice";
import { UserProfile } from "../../models/interfaces/User";

type Props = {
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
  user: UserProfile;
};
//3.validation state input with yup
const validationSchema = Yup.object({
  name: Yup.string()
    .required("نام را وارد کنید")
    .matches(/^[^<>~`@!#%^&*(){},./?$+=-_"'.,\\|0-9]+$/, {
      message:
        "نام را فقط با حروف تکمیل کنید و ازاعداد و کاراکتر ها استفاده نکنید",
    }),

  surname: Yup.string().matches(/^[^<>~`@!#%^&*(){},./?$+=-_"'.,\\|0-9]+$/, {
    message:
      "نام خانوادگی را فقط با حروف تکمیل کنید و ازاعداد و کاراکتر ها استفاده نکنید",
  }),

  userName: Yup.string().required("نام کاربری را وارد کنید"),
  email: Yup.string().email("تایپ ایمیل صحیح نیست"),

  phoneNumber: Yup.string()
    .matches(/^(\+[0-9]{1,3}[- ]?)?[0-9]{0,100}$/, {
      message: "شماره موبایل را فقط به صورت عدد وارد کنید ",
    })
    .min(11, "شماره موبایل شما باید 11 عدد باشد")
    .max(11, "شماره موبایل شما نباید بیشتر از 11 عدد باشد")
    .required("شماره موبایل را وارد کنید"),
});

export default function EditProfileForm({ setOpen, user }: Props) {
  const [title, setTitle] = useState<boolean>(
    user.title === "خانم" ? true : false
  );
  const [image, setImage] = useState<string>(user.avatar);

  const { error, loading } = useSelector((state: RootState) => state.user);
  const dispatch = useDispatch();

  // 1.managing states
  const initialValues: UserProfile = {
    avatar: user.avatar,
    email: user.email ?? "",
    name: user.name,
    phoneNumber: user.phoneNumber,
    title: user.title ?? "",
    surname: user.surname ?? "",
    userName: user.userName,
  };

  async function onSubmit(userData: UserProfile): Promise<void> {
    await dispatch(
      editProfileAsyncUser({
        ...userData,
        title: title ? "خانم" : "آقای",
        avatar: image,
      })
    ).then((res: any) => {
      if (!res.error) {
        setOpen(false);
      }
    });
  }
  const formik: FormikProps<UserProfile> = useFormik<UserProfile>({
    initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  const onChange = (event: any) => {
    const file = event.target.files;
    if (file && file[0]) {
      // Encode the file using the FileReader API
      const reader = new FileReader();
      reader.onloadend = (res) => {
        setImage(res.target?.result?.toString() || user.avatar);
      };
      reader.readAsDataURL(file[0]);
    }
  };

  return (
    <>
      <div className="flex justify-start pt-0 items-center sm:pt-3">
        <label
          htmlFor="avatar"
          className="ml-1 h-12 w-12 rounded-full overflow-hidden bg-gray-100 cursor-pointer"
        >
          <img
            src={image ?? user.avatar}
            alt="انتخاب پروفایل"
            className="h-full w-full text-gray-300"
          />
          <input
            id="avatar"
            name="avatar"
            type="file"
            className="sr-only"
            onChange={onChange}
          />
        </label>
      </div>
      <form onSubmit={formik.handleSubmit}>
        <Input formik={formik} name="userName" lable="نام کاربری" />
        {/* select title */}
        <div className="flex items-center">
          <div
            onClick={() => setTitle(!title)}
            className="flex mt-8 sm:mt-10 border rounded-[3px] p-2 sm:p-[6px]  cursor-pointer"
          >
            <div className="flex flex-col">
              <FontAwesomeIcon icon={faAngleUp} size="xs" />
              <FontAwesomeIcon icon={faAngleDown} size="xs" />
            </div>
            <span className="mr-1">{title ? "خانم" : "آقای"}</span>
          </div>
          <div className="w-full">
            <Input
              labelClass="mr-[-3.4rem]"
              formik={formik}
              name="name"
              lable="نام"
            />
          </div>
        </div>
        <Input formik={formik} name="email" lable="ایمیل" />
        <Input formik={formik} name="surname" lable="نام خانوادگی" />
        <Input
          formik={formik}
          name="phoneNumber"
          lable="شماره موبایل"
          type="tel"
        />
        <div className="pt-6 flex gap-1 items-center">
          <div className="w-1/2" onClick={() => setOpen(false)}>
            <Button color="red">انصراف</Button>
          </div>
          <div className="w-1/2">
            <Button type="submit" disabled={!formik.isValid}>
              <span>ویرایش</span>
              <Loading loading={loading} />
            </Button>
          </div>
        </div>
      </form>

      <Error error={error} />
    </>
  );
}
