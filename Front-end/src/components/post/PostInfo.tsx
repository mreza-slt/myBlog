import { useLocation } from "react-router-dom";
import { PostForm } from "../../interfaces/Post";
import timeSince from "../../util/timeSince";

export default function PostInfo(): JSX.Element {
  const location = useLocation();
  const post = location.state as PostForm;

  return (
    <div className="relative py-16 bg-white">
      <div className="relative px-4 sm:px-6 lg:px-8">
        <div className="text-lg max-w-prose mx-auto">
          <img
            className="w-full rounded-lg bg-red-50"
            src={post.image}
            alt={`imagePost ${post.id}`}
            width={1310}
            height={873}
          />
          <h1 className="mr-2 mt-6 block font-bold tracking-tight text-gray-800 sm:text-2xl">
            {post.title}
          </h1>
          <div className="border-y-[1px] mt-9">
            <div className="sm:my-2 my-1 flex items-center  font-medium text-gray-900">
              <img
                className="ml-3 pull-xs-right h-9 w-9 rounded-full"
                src={post.userAvatar}
                alt="userAvatar"
              />
              <small>
                <span className="text-red-600">نویسنده : </span>
                {post.userName}
              </small>
              <small className="border-x-[1px] sm:mx-6 sm:px-6 mx-3 px-3">
                {timeSince(post.registerDateTime)}
              </small>
              <small>بازدید : {post.numberOfVisits}</small>
            </div>
          </div>
          <p className="mt-8 text-lg text-gray-600 leading-8 text-justify">
            {post.text}
          </p>
        </div>
      </div>
    </div>
  );
}
