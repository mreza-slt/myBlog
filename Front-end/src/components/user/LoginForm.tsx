/* eslint-disable jsx-a11y/anchor-is-valid */
import Input from "../../common/Input";
import { Link, useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { FormikProps, useFormik } from "formik";
import { LoginUser } from "../../models/interfaces/User";
import { RootState } from "../../features/store";
import { useDispatch, useSelector } from "react-redux";
import { loginAsyncUser } from "../../features/user/userSlice";
import Error from "../../common/Error";

// 1.managing states
const initialValues: LoginUser = {
  userNameEmailPhone: "",
  password: "",
};

//3.validation state with yup
const validationSchema = Yup.object({
  userNameEmailPhone: Yup.string().required("نام کاربری را وارد کنید"),
  password: Yup.string()
    .min(6, "رمز عبور شما باید حداقل 6 کاراکتر باشد")
    .required("رمز عبور را وارد کنید"),
});

export default function LoginForm(): JSX.Element {
  const { error, loading } = useSelector((state: RootState) => state.user);
  const dispatch = useDispatch();

  const navigate = useNavigate();

  const onSubmit = async (userData: LoginUser) => {
    await dispatch(loginAsyncUser(userData)).then((res: any) => {
      // if (!res.error) {
      //   navigate("/");
      // }
    });
  };

  const formik: FormikProps<LoginUser> = useFormik<LoginUser>({
    initialValues: initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  return (
    <div className="h-screen bg-zinc-100">
      <div className="flex-1 flex flex-col justify-center py-12 px-4 sm:px-6 lg:flex-none lg:px-20 h-full">
        <div className="mx-auto w-full max-w-sm lg:w-96 shadow-3xl rounded-md  p-4">
          <div>
            <h1 className="text-2xl text-center font-extrabold">وبلاگ</h1>
            <p className="mt-2 text-sm text-gray">
              <Link
                to="/signup"
                className="font-medium text-indigo-600 hover:text-indigo-500"
              >
                ثبت نام
              </Link>
            </p>
          </div>

          <div className="mt-8">
            <div className="mt-6">
              <form onSubmit={formik.handleSubmit} className="space-y-6">
                <div>
                  <Input
                    formik={formik}
                    name="userNameEmailPhone"
                    lable="نام کاربری"
                  />
                </div>

                <Input
                  formik={formik}
                  name="password"
                  lable="رمز عیور"
                  type="password"
                />

                <div className="flex items-center justify-between">
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
                    ورود
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

                <Error error={error} />
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
