import classNames from "classnames";
import { Link } from "react-router-dom";
import { GetPostData } from "../models/interfaces/Post";
import timeSince from "../util/timeSince";

type PostProps = {
  post: GetPostData;
};

export default function Post({ post }: PostProps): JSX.Element {
  return (
    <Link to={`${post.id}`} state={post}>
      <div
        key={post.id}
        className="h-80 flex flex-col rounded-lg shadow-lg overflow-hidden cursor-pointer"
      >
        {post.image ? (
          <div className="flex-shrink-0 overflow-hidden">
            <img
              className="h-48 w-full object-cover hover:scale-125 duration-300"
              src={post.image}
              alt=""
            />
          </div>
        ) : (
          ""
        )}
        <div className="bg-white h-full flex flex-col justify-between">
          <div
            className={classNames(
              "mt-1 pr-1 overflow-hidden",
              post.image ? "h-[5.3rem]" : "h-[18rem]"
            )}
          >
            <p className="text-sm font-medium text-indigo-600">
              <span className="hover:underline">{post.subjectName}</span>
            </p>
            <h2 className="mt-1 text-[1rem] font-semibold text-gray-900 hover:text-red-600">
              {post.title}
            </h2>
            {post.image ? (
              ""
            ) : (
              <p className="mt-3 font-medium text-sm text-gray-600">
                {post.text}
              </p>
            )}
          </div>
          <div className="flex items-center pb-1 bg-gray-100">
            <div className="flex-shrink-0">
              <img
                className="w-9 rounded-full px-[3px] py-[2px]"
                src={post.userAvatar}
                alt=""
              />
            </div>
            <div className="ml-3">
              <h6 className="text-sm font-medium text-gray-900">
                {post.userName}
              </h6>
              <div className="flex space-x-1 text-xs font-medium text-gray-600">
                <time className="ml-2" dateTime={post.registerDateTime}>
                  {timeSince(post.registerDateTime)}
                </time>
                <span className="mx-2 font-medium text-xs text-gray-600">
                  |
                </span>
                <span>بازدید : {post.numberOfVisits}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </Link>
  );
}
