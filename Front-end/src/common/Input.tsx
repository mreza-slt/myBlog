type InputValue = {
  name: string;
  lable: string;
  type?: string;
  formik: any;
};

export default function Input({
  name,
  lable,
  type = "text",
  formik,
}: InputValue): JSX.Element {
  return (
    <div className="mt-4">
      <label
        htmlFor={name}
        className={` block text-sm font-medium text-gray-700 ${
          formik.errors[name] && formik.touched[name] ? "text-red-600" : ""
        }`}
      >
        {formik.errors[name] && formik.touched[name]
          ? formik.errors[name]
          : lable}
      </label>
      <div className="mt-1">
        <input
          {...formik.getFieldProps(name)}
          // onChange={(e) => onChange(e)}
          type={type || "text"}
          name={name}
          // value={value}
          id={name}
          autoComplete="given-name"
          className="appearance-none block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm placeholder-gray-400 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm"
        />
      </div>
    </div>
  );
}
