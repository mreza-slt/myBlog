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
        {
          "border-violet-700 bg-violet-800 hover:ring-violet-300 focus:ring-violet-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:border-violet-500 dark:text-violet-500  dark:hover:bg-violet-600 dark:focus:ring-violet-900":
            color === "violet",
        },
        {
          "border-red-700 bg-red-800 hover:ring-red-300 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:border-red-500 dark:text-red-500  dark:hover:bg-red-600 dark:focus:ring-red-900":
            color === "red",
        },
        `flex cursor-pointer justify-center w-full items-center transition-all duration-300 text-white hover:scale-[.96] border hover:ring-4 hover:outline-none focus:ring-4 focus:outline-none dark:hover:text-white`
      )}
    >
      {children}
    </button>
  );
}
