import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  posts: [],
};

const postSlice = createSlice({
  name: "post",
  initialState,
  reducers: {
    registerPost: (state, action) => {
      state.posts += action.payload;
    },
    getAllpost: (state, action) => {
      state.posts = action.payload;
    },
  },
});

export const { registerPost, getAllpost } = postSlice.actions;
export default postSlice.reducer;
