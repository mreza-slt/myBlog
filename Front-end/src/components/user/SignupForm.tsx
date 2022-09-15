/* eslint-disable jsx-a11y/anchor-is-valid */
import Input from "../../common/Input";
import * as Yup from "yup";
import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useFormik, FormikProps } from "formik";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleDown, faAngleUp } from "@fortawesome/free-solid-svg-icons";
import { SignupUser } from "../../models/interfaces/User";
import { RootState } from "../../features/store";
import { useDispatch, useSelector } from "react-redux";
import { registerAsyncUser } from "../../features/user/userSlice";
import Error from "../../common/Error";
import Loading from "../../common/Loading";
import { useQuery } from "../../hooks/useQuery";

// 1.managing states
const initialValues: SignupUser = {
  title: "",
  name: "",
  surname: "",
  userName: "",
  email: "",
  phoneNumber: "",
  password: "",
  passwordConfirm: "",
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

  password: Yup.string()
    .min(6, "رمز عبور شما باید حداقل 6 کاراکتر باشد")
    .required("رمزعبور را وارد کنید"),

  passwordConfirm: Yup.string()
    .oneOf(
      [Yup.ref("password"), null],
      "تکرار رمز عبور باید با رمز اصلی یکی باشد"
    )
    .required("تکرار رمز عبور را وارد کنید"),
});

export default function SignupForm(): JSX.Element {
  const { token, error, loading } = useSelector(
    (state: RootState) => state.user
  );
  const dispatch = useDispatch();

  const [title, setTitle] = useState<boolean>(false);
  const navigate = useNavigate();
  const query = useQuery();
  const redirect = query.get("redirect") || "/";

  useEffect(() => {
    if (token) {
      redirect === "/" ? navigate("/") : navigate(`/${redirect}`);
    }
  }, [navigate, redirect, token]);

  async function onSubmit(userData: SignupUser): Promise<void> {
    await dispatch(
      registerAsyncUser({
        ...userData,
        title: title ? "خانم" : "آقای",
      })
    ).then((res: any) => {
      if (!res.error) {
        redirect === "/" ? navigate("/") : navigate(`/${redirect}`);
      }
    });
    window.location.reload();
  }
  const formik: FormikProps<SignupUser> = useFormik<SignupUser>({
    initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  return (
    <div className="flex">
      <div className="flex flex-col justify-center w-full py-3 px-4 sm:px-6 lg:px-20 xl:px-24">
        <div className="mx-auto w-full max-w-5xl shadow-3xl px-3 rounded-lg">
          <div>
            <h2 className="text-center md:mt-6 text-3xl font-extrabold text-gray-900">
              ایجاد حساب کاربری جدید
            </h2>
            <p className="mt-2 text-sm text-gray-600">
              <Link
                to="/user/login"
                className="font-medium text-violet-600 hover:text-violet-500"
              >
                ورود
              </Link>
            </p>
          </div>

          <div>
            <div className="my-3">
              <form
                onSubmit={formik.handleSubmit}
                className="grid md:grid-cols-2 gap-3 items-center"
              >
                <Input formik={formik} name="userName" lable="نام کاربری" />
                {/* select title */}
                <div className="flex items-center">
                  <div
                    onClick={() => setTitle(!title)}
                    className="flex mt-8 sm:mt-10 border rounded-[3px] p-2 sm:p-[6px] cursor-pointer"
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
                  name="password"
                  lable="رمز عیور"
                  type="password"
                />
                <Input
                  formik={formik}
                  name="passwordConfirm"
                  lable="تکرار رمز عبور"
                  type="password"
                />
                <Input
                  formik={formik}
                  name="phoneNumber"
                  lable="شماره موبایل"
                  type="tel"
                />

                <div className="flex self-end mb-[1px] justify-between">
                  <div className="flex items-center">
                    <input
                      id="remember-me"
                      name="remember-me"
                      type="checkbox"
                      className="h-4 w-4 text-violet-600 focus:ring-violet-500 border-gray-300 rounded"
                    />
                    <label
                      htmlFor="remember-me"
                      className="ms-2 block text-sm text-gray-900"
                    >
                      مرا به خاطر بسپار
                    </label>
                  </div>

                  <div className="text-sm">
                    <a
                      href="#"
                      className="font-medium text-violet-600 hover:text-violet-500"
                    >
                      رمز عبور خود را فراموش کرده اید؟
                    </a>
                  </div>
                </div>

                <div>
                  <button
                    disabled={!formik.isValid}
                    type="submit"
                    className={`w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-violet-600 hover:bg-violet-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-violet-500 ${
                      !formik.isValid ? "opacity-50" : ""
                    }`}
                  >
                    ثبت نام
                    <Loading loading={loading} sizeClass="w-6 h-6" />
                  </button>
                </div>

                <Error error={error} />
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
