import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { LoginUser, RegisterUser } from "../../models/interfaces/User";
import { UserService } from "../../services/UserService";

const initialState = {
  user: null,
  error: null,
  loading: false,
};

export const registerAsyncUser: any = createAsyncThunk(
  "User/registerAsyncUser",
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

export const loginAsyncUser: any = createAsyncThunk(
  "User/loginAsyncUser",
  async (userData: LoginUser, { rejectWithValue }) => {
    try {
      await UserService.Login({
        userNameEmailPhone: userData.userNameEmailPhone,
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
    // register user
    builder.addCase(registerAsyncUser.fulfilled, (state, action) => {
      return {
        ...state,
        user: action.payload,
        loading: false,
        error: null,
      };
    });
    builder.addCase(registerAsyncUser.pending, (state) => {
      return { ...state, user: null, loading: true, error: null };
    });
    builder.addCase(registerAsyncUser.rejected, (state, action) => {
      return {
        ...state,
        user: null,
        loading: false,
        error: action.payload.response.data.errors,
      };
    });

    // login user
    builder.addCase(loginAsyncUser.fulfilled, (state, action) => {
      return {
        ...state,
        user: action.payload,
        loading: false,
        error: null,
      };
    });
    builder.addCase(loginAsyncUser.pending, (state) => {
      return { ...state, user: null, loading: true, error: null };
    });
    builder.addCase(loginAsyncUser.rejected, (state, action) => {
      return {
        ...state,
        user: null,
        loading: false,
        error: action.payload.response.data.errors,
      };
    });
  },
});

export default UserSlice.reducer;
