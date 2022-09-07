type InputValue = {
  name: string;
  lable: string;
  type?: string;
  labelClass?: string | null;
  formik: any;
};

export default function Input({
  name,
  lable,
  type = "text",
  labelClass = null,
  formik,
}: InputValue): JSX.Element {
  return (
    <div className="mt-4 space-y-1">
      <label
        htmlFor={name}
        className={`block text-sm font-medium  ${labelClass && labelClass} ${
          formik.errors[name] && formik.touched[name]
            ? "text-red-600"
            : "text-gray-700"
        }`}
      >
        {formik.errors[name] && formik.touched[name]
          ? formik.errors[name]
          : lable}
      </label>
      <div className="mt-1">
        <input
          {...formik.getFieldProps(name)}
          type={type || "text"}
          name={name}
          id={name}
          autoComplete="given-name"
          className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        />
      </div>
    </div>
  );
}
