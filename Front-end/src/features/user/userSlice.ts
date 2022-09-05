import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { RegisterUser } from "../../models/interfaces/User";
import { UserService } from "../../services/UserService";

const initialState = {
  Users: null,
  error: null,
  loading: false,
};

export const registerAsyncUser: any = createAsyncThunk(
  "ChildUsers/registerAsyncUser",
  async (userData: RegisterUser, { rejectWithValue }) => {
    try {
      await UserService.Register(userData);
      await UserService.Login({
        userNameEmailPhone: userData.phoneNumber,
        password: userData.password,
      });
      return userData;
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

const UserSlice = createSlice({
  name: "User",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    // get all Users
    builder.addCase(registerAsyncUser.fulfilled, (state, action) => {
      return {
        ...state,
        Users: action.payload,
        loading: false,
        error: null,
      };
    });
    builder.addCase(registerAsyncUser.pending, (state) => {
      return { ...state, Users: null, loading: true, error: null };
    });
    builder.addCase(registerAsyncUser.rejected, (state, action) => {
      return {
        ...state,
        Users: null,
        loading: false,
        error: action.payload.response.data.errors,
      };
    });
  },
});

export default UserSlice.reducer;
