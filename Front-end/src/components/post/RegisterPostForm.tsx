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
import Loading from "../../common/Loading";

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
          <Loading loading={registerLoading} sizeClass="w-5 h-5" />
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
