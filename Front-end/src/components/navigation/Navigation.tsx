import { Fragment, useState } from "react";
import { Disclosure, Menu, Transition } from "@headlessui/react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars, faXmark } from "@fortawesome/free-solid-svg-icons";
import classNames from "classnames";
import { Link } from "react-router-dom";
import RegisterPostForm from "../post/RegisterPostForm";
import DialogComponent from "../../common/Dialog";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../../features/store";
import { UserProfile } from "../../models/interfaces/User";
import { logoutAsyncUser } from "../../features/user/userSlice";
import Button from "../../common/Button";

const navigation = [
  { name: "خانه", href: "#", current: true },
  { name: "درباره ما", href: "#", current: false },
  { name: "تماس با ما", href: "#", current: false },
];

export default function Navigation() {
  const dispatch = useDispatch();

  const user: UserProfile | null = useSelector(
    (state: RootState) => state.user.user
  );

  const { token } = useSelector((state: RootState) => state.user);

  const handlerLogout = () => {
    dispatch(logoutAsyncUser());
  };

  const [open, setOpen] = useState<boolean>(false);

  return (
    <>
      <Disclosure as="nav" className="bg-white fixed top-0 z-50 w-full">
        {({ open }) => (
          <>
            <div className="max-w-7xl mx-auto px-3 sm:px-6 lg:px-8">
              <div className="flex justify-between h-16">
                <div className="flex">
                  <div className="flex items-center md:hidden">
                    {/* Mobile menu button */}
                    <Disclosure.Button className="ml-3 w-9 h-9 inline-flex items-center justify-center rounded-md text-violet-600 hover:text-white hover:bg-violet-300">
                      {open ? (
                        <FontAwesomeIcon
                          icon={faXmark}
                          className="h-6 w-6"
                          aria-hidden="true"
                        />
                      ) : (
                        <FontAwesomeIcon
                          icon={faBars}
                          className="h-6 w-6"
                          aria-hidden="true"
                        />
                      )}
                    </Disclosure.Button>
                  </div>
                  <div className="flex-shrink-0 flex items-center w-auto">
                    <svg
                      width="35"
                      height="32"
                      viewBox="0 0 35 32"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M15.2577 26.865C15.2722 27.4021 15.1794 27.9367 14.9849 28.4376C14.7903 28.9384 14.4979 29.3955 14.1247 29.782C13.7518 30.1668 13.3055 30.4728 12.8121 30.682C12.3188 30.8913 11.7886 30.9994 11.2527 31C10.7167 31.0002 10.1862 30.8924 9.69281 30.6832C9.19938 30.4739 8.75315 30.1674 8.38071 29.782C8.00681 29.396 7.71394 28.939 7.51936 28.438C7.32478 27.9371 7.23242 27.4022 7.24771 26.865C7.25671 26.167 7.44771 25.483 7.80471 24.884C8.16071 24.284 8.66771 23.79 9.27471 23.451C9.36471 23.396 9.25071 23.56 9.27471 23.451L11.1347 21.799C12.6091 20.2298 13.4326 18.1592 13.4387 16.006C13.4387 13.08 11.7277 10.105 9.26871 8.549C9.23271 8.455 9.36271 8.604 9.26871 8.549C8.6614 8.20902 8.15465 7.71469 7.79971 7.116C7.44355 6.5163 7.25145 5.83344 7.24271 5.136C7.22807 4.59872 7.32073 4.06391 7.51527 3.56287C7.70982 3.06183 8.00234 2.60462 8.37571 2.218C8.74852 1.83334 9.19469 1.52735 9.68782 1.31815C10.181 1.10894 10.711 1.00076 11.2467 1C11.7827 0.999714 12.3132 1.10742 12.8067 1.31668C13.3001 1.52595 13.7463 1.83249 14.1187 2.218C14.4928 2.60395 14.7857 3.06089 14.9803 3.56188C15.1749 4.06288 15.2672 4.59777 15.2517 5.135C15.2621 7.30708 16.1016 9.3932 17.5987 10.967L18.4157 11.767C18.7417 12.052 19.0837 12.318 19.4397 12.565C20.0607 12.895 20.5817 13.391 20.9437 13.996C21.2147 14.4489 21.391 14.9521 21.4619 15.4751C21.5328 15.9982 21.4969 16.5301 21.3563 17.0389C21.2157 17.5476 20.9734 18.0225 20.6439 18.4349C20.3145 18.8473 19.9048 19.1885 19.4397 19.438C19.3767 19.474 19.4727 19.371 19.4397 19.438C18.1836 20.2321 17.1444 21.3258 16.4157 22.621C15.6852 23.9184 15.2874 25.3764 15.2577 26.865ZM19.7407 5.123C19.7407 5.919 19.9757 6.698 20.4167 7.36C20.8561 8.02107 21.4819 8.53685 22.2147 8.842C22.9468 9.14685 23.7532 9.22661 24.5308 9.07112C25.3085 8.91563 26.0221 8.53193 26.5807 7.969C27.141 7.40452 27.5223 6.68728 27.6769 5.9071C27.8315 5.12692 27.7524 4.31848 27.4497 3.583C27.147 2.84836 26.6334 2.2199 25.9737 1.777C25.2036 1.26028 24.2776 1.02768 23.3547 1.11909C22.4318 1.2105 21.5695 1.62024 20.9157 2.278C20.1638 3.034 19.7414 4.05673 19.7407 5.123ZM23.7477 22.84C22.9557 22.84 22.1807 23.076 21.5217 23.518C20.8621 23.9609 20.3485 24.5894 20.0457 25.324C19.7428 26.0596 19.6636 26.8683 19.8182 27.6486C19.9728 28.429 20.3542 29.1464 20.9147 29.711C21.4733 30.2739 22.187 30.6576 22.9646 30.8131C23.7423 30.9686 24.5486 30.8888 25.2807 30.584C26.0142 30.2785 26.6403 29.762 27.0797 29.1C27.5945 28.3249 27.8259 27.3959 27.7348 26.47C27.6436 25.544 27.2357 24.6779 26.5797 24.018C26.2085 23.6446 25.7673 23.3482 25.2813 23.1457C24.7954 22.9432 24.2742 22.8386 23.7477 22.838V22.84ZM33.9997 15.994C33.9997 15.198 33.7647 14.42 33.3247 13.758C32.8854 13.0966 32.2597 12.5804 31.5267 12.275C30.7945 11.9699 29.9879 11.8901 29.2101 12.0456C28.4322 12.2011 27.7184 12.5849 27.1597 13.148C26.5992 13.7126 26.2178 14.43 26.0632 15.2104C25.9086 15.9907 25.9878 16.7994 26.2907 17.535C26.5934 18.2696 27.107 18.8981 27.7667 19.341C28.4249 19.783 29.1999 20.019 29.9927 20.019C30.5192 20.0182 31.0403 19.9136 31.5262 19.7111C32.0122 19.5086 32.4534 19.2123 32.8247 18.839C33.5767 18.0826 33.9991 17.0596 33.9997 15.993V15.994Z"
                        fill="#733DD8"
                      />
                      <path
                        d="M5.00724 11.9688C4.21424 11.9688 3.44024 12.2048 2.78124 12.6468C2.12146 13.09 1.60783 13.7188 1.30524 14.4538C1.0025 15.1892 0.923475 15.9977 1.07805 16.7779C1.23263 17.558 1.61394 18.2753 2.17424 18.8398C2.73364 19.4014 3.44727 19.7842 4.22459 19.9397C5.00191 20.0951 5.80788 20.0161 6.54024 19.7127C7.27315 19.4073 7.89891 18.8912 8.33824 18.2297C8.85266 17.4549 9.08385 16.5263 8.99276 15.6007C8.90166 14.6752 8.49385 13.8094 7.83824 13.1497C7.46714 12.7765 7.02609 12.48 6.54032 12.2774C6.05455 12.0747 5.53358 11.9699 5.00724 11.9688V11.9688Z"
                        fill="#733DD8"
                      />
                    </svg>
                  </div>
                  <div className="hidden md:mr-6 md:flex md:items-center md:space-x-4">
                    {navigation.map((item) => (
                      <Link
                        key={item.name}
                        to="/"
                        className={classNames(
                          item.current ? "bg-gray-200" : "hover:bg-gray-200",
                          "px-3 py-2 rounded-md text-sm font-medium"
                        )}
                        aria-current={item.current ? "page" : undefined}
                      >
                        {item.name}
                      </Link>
                    ))}
                  </div>
                </div>
                <div className="flex items-center">
                  <div onClick={() => setOpen(true)} className="flex-shrink-0">
                    <Button>پست جدید</Button>
                  </div>
                  <div className="md:mr-4 flex-shrink-0 flex items-center">
                    {/* Profile dropdown */}
                    {user && token !== null ? (
                      <Menu as="div" className="mr-3 relative">
                        <div>
                          <Menu.Button className="flex text-sm rounded-full focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-white">
                            <img
                              className="h-10 w-10 rounded-full"
                              src={user.avatar}
                              alt=""
                            />
                          </Menu.Button>
                        </div>
                        <Transition
                          as={Fragment}
                          enter="transition ease-out duration-200"
                          enterFrom="transform opacity-0 scale-95"
                          enterTo="transform opacity-100 scale-100"
                          leave="transition ease-in duration-75"
                          leaveFrom="transform opacity-100 scale-100"
                          leaveTo="transform opacity-0 scale-95"
                        >
                          <Menu.Items className="origin-top-left absolute left-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none">
                            <Menu.Item>
                              {({ active }) => (
                                <Link
                                  to="/user/profile"
                                  className={classNames(
                                    active ? "bg-gray-100" : "",
                                    "flex justify-between items-center px-4 py-2 text-sm text-gray-700"
                                  )}
                                >
                                  <div>
                                    <img
                                      className="h-10 w-10 rounded-full"
                                      src={user.avatar}
                                      alt=""
                                    />
                                  </div>
                                  <div>
                                    <p>{user.name}</p>
                                    <p>{user.phoneNumber}</p>
                                  </div>
                                </Link>
                              )}
                            </Menu.Item>
                            <Menu.Item>
                              {({ active }) => (
                                <Link
                                  onClick={() => handlerLogout}
                                  to="user/logout"
                                  className={classNames(
                                    active ? "bg-gray-100" : "",
                                    "block px-4 py-2 text-sm text-gray-700"
                                  )}
                                >
                                  خروج
                                </Link>
                              )}
                            </Menu.Item>
                          </Menu.Items>
                        </Transition>
                      </Menu>
                    ) : (
                      <Link
                        to="user/login"
                        className="flex justify-between items-center"
                      >
                        <div className="flex items-center justify-center text-violet-600 rounded-full w-12 h-12 hover:bg-violet-50">
                          <svg
                            stroke="currentColor"
                            fill="none"
                            stroke-width="0"
                            viewBox="0 0 24 24"
                            height="24"
                            width="24"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              strokeWidth="2"
                              d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"
                            ></path>
                          </svg>
                        </div>

                        <span>ورود</span>
                      </Link>
                    )}
                  </div>
                </div>
              </div>
            </div>

            <Disclosure.Panel className="md:hidden">
              <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
                {navigation.map((item) => (
                  <Link
                    key={item.name}
                    to="/"
                    className={classNames(
                      item.current
                        ? "bg-gray-900 text-white"
                        : "text-gray-300 hover:bg-gray-700 hover:text-white",
                      "block px-3 py-2 rounded-md text-base font-medium"
                    )}
                    aria-current={item.current ? "page" : undefined}
                  >
                    {item.name}
                  </Link>
                ))}
              </div>
            </Disclosure.Panel>
          </>
        )}
      </Disclosure>
      <DialogComponent setOpen={setOpen} open={open}>
        {user && token !== null ? (
          <RegisterPostForm setOpen={setOpen} />
        ) : (
          <>
            <h1 className="text-center mb-6 block font-bold tracking-tight text-gray-800 sm:text-xl">
              برای ثبت پست جدید ابتدا باید وارد شوید
            </h1>
            <Link
              onClick={() => setOpen(false)}
              to="user/login"
              className="flex justify-center items-center w-full transition-all duration-300 text-violet-700 hover:text-white border border-violet-700 hover:bg-violet-800 focus:ring-4 focus:outline-none focus:ring-violet-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center mb-2 dark:border-violet-500 dark:text-violet-500 dark:hover:text-white dark:hover:bg-violet-600 dark:focus:ring-violet-900"
            >
              ورود
            </Link>
          </>
        )}
      </DialogComponent>
    </>
  );
}
