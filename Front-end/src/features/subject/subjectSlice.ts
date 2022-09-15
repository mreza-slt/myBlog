import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { SubjectService } from "../../services/SubjectService";

const initialState = {
  Subjects: [],
  ChildSubjects: [],
  error: null,
  loading: false,
};

export const getAsyncSubjects: any = createAsyncThunk(
  "Subjects/getAsyncSubjects",
  async (_, { rejectWithValue }) => {
    try {
      const { data } = await SubjectService.GetAll(0);
      return data;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

export const getAsyncChildSubjects: any = createAsyncThunk(
  "ChildSubjects/getAsyncChildSubjects",
  async (parentId: number, { rejectWithValue }) => {
    try {
      const { data } = await SubjectService.GetAll(parentId);
      return data;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

const SubjectSlice = createSlice({
  name: "Subject",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    // get all subjects
    builder.addCase(getAsyncSubjects.fulfilled, (state, action) => {
      return {
        ...state,
        Subjects: action.payload,
        loading: false,
        error: null,
      };
    });
    builder.addCase(getAsyncSubjects.pending, (state) => {
      return { ...state, Subjects: [], loading: true, error: null };
    });
    builder.addCase(getAsyncSubjects.rejected, (state, action) => {
      return {
        ...state,
        Subjects: [],
        loading: false,
        error: action.payload.response.data.errors,
      };
    });

    // get all childSubjects
    builder.addCase(getAsyncChildSubjects.fulfilled, (state, action) => {
      return {
        ...state,
        ChildSubjects: action.payload,
        loading: false,
        error: null,
      };
    });
    builder.addCase(getAsyncChildSubjects.pending, (state) => {
      return { ...state, ChildSubjects: [], loading: true, error: null };
    });
    builder.addCase(getAsyncChildSubjects.rejected, (state, action) => {
      return {
        ...state,
        ChildSubjects: [],
        loading: false,
        error: action.payload.response.data.errors,
      };
    });
  },
});

export default SubjectSlice.reducer;
