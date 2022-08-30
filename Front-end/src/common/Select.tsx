import { useEffect, useMemo } from "react";
import Select from "react-select";
import { Option } from "../interfaces/SelectOption";
import { SubjectForm } from "../interfaces/Subject";

interface SelectValues {
  formik: any;
  name: string;
  lable: string;
  subjects: SubjectForm[];
}

export default function SelectBox({
  formik,
  name,
  lable,
  subjects,
}: SelectValues): JSX.Element {
  let options: Option[] = useMemo(() => [], []);

  useEffect(() => {
    options.splice(0, options.length);
    for (var i = 0; i < subjects.length; i++) {
      options[i] = { value: subjects[i].id, label: subjects[i].name };
    }
  }, [options, subjects]);

  return (
    <>
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
      <Select
        className="mt-2"
        options={options}
        {...formik.getFieldProps(name)}
        name={name}
        value={
          options ? options.find((option) => option.value === formik.value) : ""
        }
        placeholder="انتخاب کنید"
        onChange={(option: Option) => formik.setFieldValue(name, option.value)}
      />
    </>
  );
}
