import { useEffect, useState } from "react";
import { SetPostData } from "../../models/interfaces/Post";
import { FormikProps, useFormik } from "formik";
import * as Yup from "yup";
import Input from "../../common/Input";
import Select from "../../common/Select";
import { useDispatch, useSelector } from "react-redux";
import { registerAsyncPost } from "../../features/post/postSlice";
import { RootState } from "../../features/store";
import {
  getAsyncChildSubjects,
  getAsyncSubjects,
} from "../../features/subject/subjectSlice";
import Error from "../../common/Error";
import Loading from "../../common/Loading";
import Button from "../../common/Button";

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
    setOpen(false);
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
      <h1 className="text-center block font-bold tracking-tight text-violet-500 sm:text-2xl">
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
            className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-violet-500 focus:border-violet-500 sm:text-sm"
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
        <div className="pt-6 flex gap-1 items-center">
          <div onClick={() => setOpen(false)} className="w-1/2">
            <Button color="red">انصراف</Button>
          </div>
          <div className="w-1/2">
            <Button type="submit" disabled={!formik.isValid}>
              <span>ثبت</span>
              <Loading loading={registerLoading} sizeClass="w-6 h-6" />
            </Button>
          </div>
        </div>
      </form>

      <Error error={error} />
    </>
  );
}
