import { useLocation } from "react-router-dom";
import { GetPostData } from "../../models/interfaces/Post";
import timeSince from "../../util/timeSince";

export default function PostInfo(): JSX.Element {
  const location = useLocation();
  const post = location.state as GetPostData;

  return (
    <div className="relative pt-6 sm:mt-12 sm:py-16 bg-gray-100">
      <div className="relative sm:px-6 lg:px-8">
        <div className="bg-white rounded-lg text-lg max-w-prose mx-auto">
          {post.image ? (
            <img
              className="w-full bg-red-50 rounded-t-lg"
              src={post.image}
              alt=""
              width={1310}
              height={873}
            />
          ) : (
            ""
          )}

          <div className="p-3">
            <h1 className="mt-6 block font-bold tracking-tight text-gray-800 sm:text-xl">
              {post.title}
            </h1>
            <div className="border-y-[1px] mt-9">
              <div className="sm:my-2 my-1 flex justify-start items-center font-medium text-gray-900">
                <img
                  className="ml-3 h-9 w-9 rounded-full"
                  src={post.userAvatar}
                  alt=""
                />
                <small>
                  <span className="text-red-600">نویسنده : </span>
                  {post.userName}
                </small>
                <small className="border-x-[1px] sm:mx-3 sm:px-3 mx-1 px-1">
                  {timeSince(post.registerDateTime)}
                </small>
                <small>بازدید : {post.numberOfVisits}</small>
              </div>
            </div>
            <p className="mt-8 text-sm text-gray-600 leading-8 text-justify">
              {post.text}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
