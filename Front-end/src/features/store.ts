import { configureStore } from "@reduxjs/toolkit";
import PostReducer from "./post/postSlice";
import SubjectReducer from "./subject/subjectSlice";

export const store = configureStore({
  reducer: {
    post: PostReducer,
    subject: SubjectReducer,
  },
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;
