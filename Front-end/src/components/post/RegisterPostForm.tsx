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

type Props = {
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
};

// 1.managing states
const initialValues: SetPostData = {
  title: "",
  text: "",
  image: "",
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

export default function RegisterPostForm({ setOpen }: Props) {
  const { error, registerLoading } = useSelector(
    (state: RootState) => state.post
  );
  const { Subjects, ChildSubjects } = useSelector(
    (state: RootState) => state.subject
  );
  const dispatch = useDispatch();

  const [image, setImage] = useState<string>();
  const onChange = (event: any) => {
    const file = event.target.files;
    if (file && file[0]) {
      // Encode the file using the FileReader API
      const reader = new FileReader();
      reader.onloadend = (res) => {
        setImage(res.target?.result?.toString()!);
      };
      reader.readAsDataURL(file[0]);
    }
  };

  const onSubmit = async (postData: SetPostData) => {
    dispatch(registerAsyncPost({ ...postData, image: image })).then(
      (res: any) => {
        if (!res.error) {
          setOpen(false);
        }
      }
    );
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
        <div className="flex justify-start items-center">
          <label
            htmlFor="image"
            className="ml-1 h-24 w-24 overflow-hidden bg-gray-100 cursor-pointer"
          >
            <input
              id="image"
              name="image"
              type="file"
              className="sr-only"
              onChange={onChange}
            />
            <img
              src={image ?? ""}
              alt="انتخاب عکس"
              className="h-full w-full text-gray-300"
            />
          </label>
          {!image && formik.touched.image ? (
            <label
              htmlFor="image"
              className="text-sm text-red-600 cursor-pointer"
            >
              یک عکس را انتخاب کنید
            </label>
          ) : (
            ""
          )}
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
            <Button type="submit">
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
