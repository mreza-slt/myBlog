import classNames from "classnames";

export default function Button({
  type = "button",
  children,
  disabled = false,
  color = "violet",
}: {
  children: any;
  type?: "button" | "submit";
  disabled?: boolean;
  color?: string;
}) {
  return (
    <button
      disabled={disabled}
      type={type}
      className={classNames(
        { "opacity-70": disabled },
        `flex cursor-pointer justify-center w-full items-center transition-all duration-300 text-white hover:scale-[.96] border border-${color}-700 bg-${color}-800 hover:ring-4 hover:outline-none hover:ring-${color}-300 focus:ring-4 focus:outline-none focus:ring-${color}-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:border-${color}-500 dark:text-${color}-500 dark:hover:text-white dark:hover:bg-${color}-600 dark:focus:ring-${color}-900`
      )}
    >
      {children}
    </button>
  );
}
