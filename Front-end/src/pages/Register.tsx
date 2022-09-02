/* eslint-disable jsx-a11y/anchor-is-valid */
import Input from "../common/Input";
import * as Yup from "yup";
import { useState } from "react";
import { UserService } from "../services/UserService";
import { Link, useNavigate } from "react-router-dom";

import { useFormik, FormikProps } from "formik";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleDown, faAngleUp } from "@fortawesome/free-solid-svg-icons";
import { RegisterUser } from "../models/interfaces/User";

// 1.managing states
const initialValues: RegisterUser = {
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

export default function Register(): JSX.Element {
  const [error, setError] = useState<Object | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [title, setTitle] = useState<boolean>(false);

  const history = useNavigate();

  async function onSubmit(userData: RegisterUser): Promise<void> {
    setLoading(true);
    try {
      await UserService.Register({
        ...userData,
        title: title ? "خانم" : "آقای",
      });
      await UserService.Login({
        userNameEmailPhone: userData.phoneNumber,
        password: userData.password,
      });
      setError(null);
      history("/");
    } catch (err: any) {
      setError(err.response.data.errors);
    }
    setLoading(false);
  }
  const formik: FormikProps<RegisterUser> = useFormik<RegisterUser>({
    initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  return (
    <div className="h-screen flex">
      <div className="flex flex-col justify-center w-full py-3 px-4 sm:px-6 lg:px-20 xl:px-24">
        <div className="mx-auto w-full max-w-5xl shadow-3xl px-3 rounded-lg">
          <div>
            <h2 className="md:mt-6 text-3xl font-extrabold text-gray-900">
              ایجاد حساب کاربری جدید
            </h2>
            <p className="mt-2 text-sm text-gray-600">
              <Link
                to="/login"
                className="font-medium text-indigo-600 hover:text-indigo-500"
              >
                ورود
              </Link>
            </p>
          </div>

          <div className="mt-8">
            <div className="my-3">
              <form
                onSubmit={formik.handleSubmit}
                className="grid md:grid-cols-2 gap-3 items-center"
              >
                <Input formik={formik} name="userName" lable="نام کاربری" />
                {/* select title */}
                <div role="button" className="flex items-center">
                  <div
                    onClick={() => setTitle(!title)}
                    className="flex mt-10 border rounded-[3px] p-2 sm:p-[6px]"
                  >
                    <div className="flex flex-col">
                      <FontAwesomeIcon icon={faAngleUp} size="xs" />
                      <FontAwesomeIcon icon={faAngleDown} size="xs" />
                    </div>
                    <span className="mr-1">{title ? "خانم" : "آقای"}</span>
                  </div>
                  <div className="w-full">
                    <Input formik={formik} name="name" lable="نام" />
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

                <div className="flex self-end mb-[1px] justify-between">
                  <div className="flex items-center">
                    <input
                      id="remember-me"
                      name="remember-me"
                      type="checkbox"
                      className="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
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
                      className="font-medium text-indigo-600 hover:text-indigo-500"
                    >
                      رمز عبور خود را فراموش کرده اید؟
                    </a>
                  </div>
                </div>

                <div>
                  <button
                    disabled={!formik.isValid}
                    type="submit"
                    className={`w-full flex justify-center py-2 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 ${
                      !formik.isValid ? "opacity-50" : ""
                    }`}
                  >
                    ثبت نام
                    {loading ? (
                      <div role="status">
                        <svg
                          className="inline mr-3 w-6 h-6 text-gray-200 animate-spin dark:text-gray-600 fill-indigo-600"
                          viewBox="0 0 100 101"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
                            fill="currentColor"
                          />
                          <path
                            d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
                            fill="currentFill"
                          />
                        </svg>
                      </div>
                    ) : (
                      ""
                    )}
                  </button>
                </div>
                <div className="mt-4">
                  {error &&
                    Object.values(error).map((value: string) => (
                      <div key={value}>
                        <span className="text-red-600">{value}</span>
                        <br />
                      </div>
                    ))}
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
