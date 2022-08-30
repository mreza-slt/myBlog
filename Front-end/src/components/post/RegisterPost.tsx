import { Fragment, useEffect, useState } from "react";
import { Dialog, Transition } from "@headlessui/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faXmark } from "@fortawesome/free-solid-svg-icons";
import { RegisterPostForm } from "../../interfaces/Post";
import { FormikProps, useFormik } from "formik";
import * as Yup from "yup";
import { PostService } from "../../services/PostService";
import Input from "../../common/Input";
import Select from "../../common/Select";
import { SubjectService } from "../../services/SubjectService";
import { SubjectForm } from "../../interfaces/Subject";

// 1.managing states
const initialValues: RegisterPostForm = {
  title: "",
  text: "",
  image: null,
  subjectId: "",
  childSubjectId: "",
};

//3.validation state input with yup
const validationSchema = Yup.object({
  title: Yup.string().required("عنوان را وارد کنید"),

  text: Yup.string().required("متن پست را وارد"),
  image: Yup.mixed().required("یک تصویر برای پست خود انتخاب کنید"),
  subjectId: Yup.string().required("دسته بندی را مشخص کنید"),
  childSubjectId: Yup.string().required("موضوع را مشخص کنید"),
});

export default function RegisterPost() {
  const [loading, setLoading] = useState<boolean>(false);
  const [image, setImage] = useState<File>();
  const [error, setError] = useState<Object | null>(null);
  const [open, setOpen] = useState(true);
  const [subjects, setSubjects] = useState<SubjectForm[]>([]);
  const [childSubjects, setChildSubjects] = useState<SubjectForm[]>([]);

  const onSubmit = async (postData: RegisterPostForm) => {
    setLoading(true);
    try {
      await PostService.Register({ ...postData, image: image! });
      setError(null);
    } catch (err: any) {
      setError(err.response.data.errors);
    }
    setLoading(false);
  };

  const formik: FormikProps<RegisterPostForm> = useFormik<RegisterPostForm>({
    initialValues,
    onSubmit,
    validationSchema,
    validateOnMount: true,
  });

  const getSubjects = async (parentId: number | undefined) => {
    const subject = await SubjectService.GetAll(parentId);
    return subject;
  };

  useEffect(() => {
    getSubjects(0)
      .then(({ data }) => {
        setSubjects(data);
      })
      .catch((err) => {
        // TODO
        throw err;
      });
  }, []);

  const subjectId: number | null = formik.getFieldProps("subjectId").value;
  useEffect(() => {
    if (subjectId) {
      getSubjects(subjectId)
        .then(({ data }) => {
          setChildSubjects(data);
        })
        .catch((err) => {
          // TODO
          throw err;
        });
    }
  }, [subjectId, subjects]);

  const onChange = (event: any) => {
    setImage(event.target.files[0]);
  };

  return (
    <Transition.Root show={open} as={Fragment}>
      <Dialog
        as="div"
        className="fixed z-10 inset-0 overflow-y-auto"
        onClose={setOpen}
      >
        <div className="flex justify-center pt-4 px-4 pb-20 text-center sm:block sm:p-0">
          <Transition.Child
            as={Fragment}
            enter="ease-out duration-300"
            enterFrom="opacity-0"
            enterTo="opacity-100"
            leave="ease-in duration-200"
            leaveFrom="opacity-100"
            leaveTo="opacity-0"
          >
            <Dialog.Overlay className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
          </Transition.Child>

          {/* This element is to trick the browser into centering the modal contents. */}
          <span
            className="hidden sm:inline-block sm:align-middle sm:h-screen"
            aria-hidden="true"
          >
            &#8203;
          </span>
          <Transition.Child
            as={Fragment}
            enter="ease-out duration-300"
            enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
            enterTo="opacity-100 translate-y-0 sm:scale-100"
            leave="ease-in duration-200"
            leaveFrom="opacity-100 translate-y-0 sm:scale-100"
            leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
          >
            <div className="relative inline-block align-top bg-white rounded-lg px-4 pt-5 pb-4 text-right shadow-xl transform transition-all sm:my-8 sm:max-w-lg sm:w-full sm:p-6">
              <div className="hidden sm:block absolute top-0 right-0 pt-4 pr-4">
                <button
                  type="button"
                  className="bg-white rounded-md text-gray-400 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                  onClick={() => setOpen(false)}
                >
                  <span className="sr-only">Close</span>
                  <FontAwesomeIcon
                    icon={faXmark}
                    className="h-6 w-6"
                    aria-hidden="true"
                  />
                </button>
              </div>
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
                    className={` block text-sm font-medium text-gray-700 ${
                      formik.errors.image && formik.touched.image
                        ? "text-red-600"
                        : ""
                    }`}
                  >
                    {formik.errors.image && formik.touched.image
                      ? formik.errors.image
                      : "تصویر"}
                  </label>
                  <input
                    {...formik.getFieldProps("image")}
                    accept="image/*"
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
                      subjects={subjects}
                    />
                  </div>
                  <div className="w-1/2">
                    <Select
                      formik={formik}
                      name="childSubjectId"
                      lable="موضوع"
                      subjects={childSubjects}
                    />
                  </div>
                </div>
                <button
                  type="submit"
                  className="inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-md shadow-sm text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                  ثبت
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
                </button>{" "}
                <button
                  type="button"
                  className="inline-flex items-center px-3 py-2 border border-transparent text-sm leading-4 font-medium rounded-md shadow-sm text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                >
                  انصراف
                </button>
              </form>
              <div className="mt-4">
                {error &&
                  Object.values(error).map((value: string) => (
                    <div key={value}>
                      <span className="text-red-600">{value}</span>
                      <br />
                    </div>
                  ))}
              </div>
            </div>
          </Transition.Child>
        </div>
      </Dialog>
    </Transition.Root>
  );
}
