import { useEffect, useState } from "react";
import { SetPostData } from "../../models/interfaces/Post";
import { FormikProps, useFormik } from "formik";
import * as Yup from "yup";
import Input from "../../common/Input";
import Select from "../../common/Select";
import classNames from "classnames";
import { useDispatch, useSelector } from "react-redux";
import { registerAsyncPost } from "../../features/post/postSlice";
import { RootState } from "../../features/store";
import {
  getAsyncChildSubjects,
  getAsyncSubjects,
} from "../../features/subject/subjectSlice";
import Error from "../../common/Error";

// 1.managing states
const initialValues: SetPostData = {
  title: "",
  text: "",
  subjectId: "",
  childSubjectId: "",
};

//3.validation state input with yup
const validationSchema = Yup.object({
  title: Yup.string().required("عنوان را وارد کنید"),
  text: Yup.string().required("متن پست را وارد"),
  subjectId: Yup.string().required("دسته بندی را مشخص کنید"),
  childSubjectId: Yup.string().required("موضوع را مشخص کنید"),
});

type Props = {
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

export default function RegisterPostForm({ setOpen }: Props) {
  const { error, registerLoading } = useSelector(
    (state: RootState) => state.post
  );
  const { Subjects, ChildSubjects } = useSelector(
    (state: RootState) => state.subject
  );
  const dispatch = useDispatch();

  const [image, setImage] = useState<File | null>(null);

  const onSubmit = async (postData: SetPostData) => {
    dispatch(registerAsyncPost({ ...postData, image: image }));
  };

  const formik: FormikProps<SetPostData> = useFormik<SetPostData>({
    initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  useEffect(() => {
    dispatch(getAsyncSubjects());
  }, [dispatch]);

  const subjectId: number | null = formik.getFieldProps("subjectId").value;
  useEffect(() => {
    if (subjectId) {
      dispatch(getAsyncChildSubjects(subjectId));
    }
  }, [dispatch, subjectId]);

  const onChange = (event: any) => {
    setImage(event.target.files[0]);
  };

  return (
    <>
      <h1 className="text-center block font-bold tracking-tight text-indigo-500 sm:text-2xl">
        ایجاد پست جدید
      </h1>
      <form onSubmit={formik.handleSubmit} className="space-y-6">
        <div className="space-y-1">
          <Input formik={formik} name="title" lable="عنوان" />
        </div>
        <div className="space-y-1">
          <Input formik={formik} name="text" lable="متن" />
        </div>
        <div className="space-y-1 ">
          <label
            htmlFor={"image"}
            className="block text-sm font-medium text-gray-700"
          >
            تصویر
          </label>
          <input
            accept="image/*"
            id="image"
            name="image"
            type="file"
            className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
            onChange={onChange}
          />
        </div>
        <div className="flex gap-1 mt-3">
          <div className="w-1/2">
            <Select
              formik={formik}
              name="subjectId"
              lable="دسته بندی"
              subjects={Subjects}
            />
          </div>
          <div className="w-1/2">
            <Select
              formik={formik}
              name="childSubjectId"
              lable="موضوع"
              subjects={ChildSubjects}
            />
          </div>
        </div>
        <button
          disabled={!formik.isValid}
          type="submit"
          className={classNames(
            { "opacity-70": !formik.isValid },
            "inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-md shadow-sm text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          )}
        >
          ثبت
          {registerLoading ? (
            <svg
              className="inline mr-2 w-5 h-5 text-gray-200 animate-spin dark:text-gray-600 fill-indigo-600"
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
          ) : (
            ""
          )}
        </button>
        <button
          onClick={() => setOpen(false)}
          type="button"
          className="inline-flex items-center mr-3 px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-md shadow-sm text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
        >
          انصراف
        </button>
      </form>

      <Error error={error} />
    </>
  );
}
