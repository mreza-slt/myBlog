import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "../../features/store";
import { getAsyncPosts } from "../../features/post/postSlice";
import { GetPostData } from "../../models/interfaces/Post";
import Loading from "../../common/Loading";
import timeSince from "../../util/timeSince";
import { Link } from "react-router-dom";
import classNames from "classnames";

export default function GetAllPost(): JSX.Element {
  const { posts, loading } = useSelector((state: RootState) => state.post);
  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(getAsyncPosts());
  }, [dispatch]);

  return (
    <>
      <div className="min-h-screen flex items-center justify-center bg-gray-100 sm-pt-16 sm:pb-20 sm:px-6 lg:pt-24 lg:pb-28 lg:px-8">
        {loading ? (
          <Loading loading={loading} sizeClass="w-12" />
        ) : (
          <div className="relative max-w-7xl mx-auto">
            {posts && posts.length > 0 ? (
              <div className="mt-6 sm:mt-12 max-w-lg mx-auto grid gap-5 xl:grid-cols-4 lg:grid-cols-3 md:grid-cols-2 md:max-w-none">
                {posts.map((post: GetPostData) => (
                  <Post key={post.id} post={post} />
                ))}
              </div>
            ) : (
              "هنوز هیچ پستی ثبت نشده است"
            )}
          </div>
        )}
      </div>
    </>
  );
}

type PostProps = {
  post: GetPostData;
};

function Post({ post }: PostProps): JSX.Element {
  return (
    <Link to={`${post.id}`} state={post}>
      <div
        key={post.id}
        className="h-80 flex flex-col rounded-lg shadow-lg overflow-hidden cursor-pointer hover:shadow-3xl"
      >
        {post.image ? (
          <div className="flex-shrink-0 overflow-hidden">
            <img
              className="h-48 w-full object-fill hover:scale-125 duration-300"
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
            <p className="text-sm font-medium text-violet-600">
              <span className="hover:underline">{post.subjectName}</span>
            </p>
            <h2 className="mt-1 text-[0.9rem] font-semibold text-gray-900 hover:text-red-600">
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
                className="w-9 h-9 rounded-full"
                src={post.userAvatar}
                alt=""
              />
            </div>
            <div className="mr-3">
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
