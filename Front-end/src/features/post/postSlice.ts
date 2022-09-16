import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { PostService } from "../../services/PostService";
import { SetPostData } from "../../models/interfaces/Post";

const initialState = {
  posts: [],
  error: null,
  loading: false,
  registerLoading: false,
};

export const getAsyncPosts: any = createAsyncThunk(
  "posts/getAsyncPosts",
  async (_, { rejectWithValue }) => {
    try {
      const { data } = await PostService.GetAll();
      return data;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

export const registerAsyncPost: any = createAsyncThunk(
  "posts/registerAsyncPost",
  async (postData: SetPostData, { rejectWithValue }) => {
    try {
      const response = await PostService.Register(postData);
      const { data } = await PostService.Get(response.data);
      return data;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

const postSlice = createSlice({
  name: "post",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder.addCase(getAsyncPosts.fulfilled, (state, action) => {
      return { ...state, posts: action.payload, loading: false, error: null };
    });
    builder.addCase(getAsyncPosts.pending, (state) => {
      return { ...state, loading: true, error: null };
    });
    builder.addCase(getAsyncPosts.rejected, (state, action) => {
      return {
        ...state,
        posts: [],
        loading: false,
        error: action.payload.response.data.errors,
      };
    });
    // add new post
    builder.addCase(
      registerAsyncPost.fulfilled,
      (state, action: { payload: never }) => {
        state.posts.push(action.payload);
        state.registerLoading = false;
      }
    );
    builder.addCase(registerAsyncPost.pending, (state) => {
      state.registerLoading = true;
      state.error = null;
    });
    builder.addCase(registerAsyncPost.rejected, (state, action) => {
      state.registerLoading = false;
      state.error = action.payload.response.data.errors;
    });
  },
});

export default postSlice.reducer;
