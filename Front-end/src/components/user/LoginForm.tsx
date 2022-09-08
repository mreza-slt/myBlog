/* eslint-disable jsx-a11y/anchor-is-valid */
import Input from "../../common/Input";
import { Link, useLocation, useNavigate } from "react-router-dom";
import * as Yup from "yup";
import { FormikProps, useFormik } from "formik";
import { LoginUser } from "../../models/interfaces/User";
import { RootState } from "../../features/store";
import { useDispatch, useSelector } from "react-redux";
import { loginAsyncUser } from "../../features/user/userSlice";
import Error from "../../common/Error";
import Loading from "../../common/Loading";
import { AuthRoutes, NonAuthRoutes } from "../../models/enums/AuthRoutes";
import { AuthState } from "../../models/interfaces/Auth";

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
  const location = useLocation();
  const user = location.state as AuthState;

  const onSubmit = async (userData: LoginUser) => {
    await dispatch(loginAsyncUser(userData)).then((res: any) => {
      if (!res.error) {
        navigate(user?.requestedPath ?? NonAuthRoutes.posts);
      }
    });
  };

  const formik: FormikProps<LoginUser> = useFormik<LoginUser>({
    initialValues: initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  return (
    <div className="bg-zinc-100">
      <div className="flex-1 flex flex-col justify-center py-12 px-4 sm:px-6 lg:flex-none lg:px-20 h-full">
        <div className="mx-auto w-full max-w-sm lg:w-96 shadow-3xl rounded-md  p-4">
          <div>
            <h1 className="text-2xl text-center font-extrabold">وبلاگ</h1>
            <p className="mt-2 text-sm text-gray">
              <Link
                to="/user/signup"
                className="font-medium text-violet-600 hover:text-violet-500"
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
                    ورود
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
