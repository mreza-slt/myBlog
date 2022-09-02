import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { PostService } from "../../services/PostService";
import { PostDataForm } from "../../models/interfaces/Post";

const initialState = {
  posts: [],
  error: null,
  loading: false,
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
  async (postData: PostDataForm, { rejectWithValue }) => {
    try {
      const { data } = await PostService.Register(postData);
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
      return { ...state, posts: [], loading: true, error: null };
    });
    builder.addCase(getAsyncPosts.rejected, (state, action) => {
      return { ...state, posts: [], loading: false, error: action.error };
    });
    // add post
    builder.addCase(
      registerAsyncPost.fulfilled,
      (state, action: { payload: never }) => {
        state.posts.push(action.payload);
        return { ...state, loading: false, error: null };
      }
    );
    builder.addCase(registerAsyncPost.pending, (state) => {
      return { ...state, loading: true, error: null };
    });
    builder.addCase(registerAsyncPost.rejected, (state, action) => {
      return { ...state, loading: false, error: action.error };
    });
  },
});

export default postSlice.reducer;
