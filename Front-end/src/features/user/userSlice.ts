import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import Cookies from "js-cookie";
import {
  LoginUser,
  SignupUser,
  UserProfile,
} from "../../models/interfaces/User";
import { UserService } from "../../services/UserService";

const initialState: {
  token: string | null;
  user: UserProfile | null;
  error: {} | null;
  loading: boolean;
} = {
  token: Cookies.get("MyBlog") || null,
  user: JSON.parse(localStorage.getItem("stateUser")!),
  error: null,
  loading: false,
};

if (initialState.token === null) {
  localStorage.removeItem("stateUser");
  initialState.user = null;
}

export const registerAsyncUser: any = createAsyncThunk(
  "User/registerAsyncUser",
  async (userData: SignupUser, { rejectWithValue }) => {
    try {
      await UserService.Register(userData);

      const { data } = await UserService.Login({
        userNameEmailPhone: userData.phoneNumber,
        password: userData.password,
      });

      localStorage.setItem("stateUser", JSON.stringify(data));

      return { userData: userData, token: Cookies.get("MyBlog") };
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

export const editProfileAsyncUser: any = createAsyncThunk(
  "User/editProfileAsyncUser",
  async (userData: UserProfile, { rejectWithValue }) => {
    try {
      await UserService.EditProfile(userData);
      localStorage.removeItem("stateUser");
      localStorage.setItem("stateUser", JSON.stringify(userData));
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
      const { data } = await UserService.Login({
        userNameEmailPhone: userData.userNameEmailPhone,
        password: userData.password,
      });
      localStorage.setItem("stateUser", JSON.stringify(data));

      return { userData: data, token: Cookies.get("MyBlog") };
    } catch (error: any) {
      return rejectWithValue(error);
    }
  }
);

export const logoutAsyncUser: any = createAsyncThunk(
  "User/logoutAsyncUser",
  async (_, { rejectWithValue }) => {
    try {
      await UserService.Logout();
      localStorage.removeItem("stateUser");
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
        token: action.payload.token,
        user: action.payload.userData,
        loading: false,
        error: null,
      };
    });
    builder.addCase(registerAsyncUser.pending, (state) => {
      return { ...state, loading: true, error: null };
    });
    builder.addCase(registerAsyncUser.rejected, (state, action) => {
      return {
        ...state,
        user: null,
        loading: false,
        error: action.payload.response.data.errors,
      };
    });

    // update user profile
    builder.addCase(editProfileAsyncUser.fulfilled, (state, action) => {
      return {
        ...state,
        user: action.payload,
        loading: false,
        error: null,
      };
    });
    builder.addCase(editProfileAsyncUser.pending, (state) => {
      return { ...state, loading: true, error: null };
    });
    builder.addCase(editProfileAsyncUser.rejected, (state, action) => {
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
        token: action.payload.token,
        user: action.payload.userData,
        loading: false,
        error: null,
      };
    });
    builder.addCase(loginAsyncUser.pending, (state) => {
      return { ...state, loading: true, error: null };
    });
    builder.addCase(loginAsyncUser.rejected, (state, action) => {
      return {
        ...state,
        user: null,
        loading: false,
        error: action.payload.response.data.errors,
      };
    });

    // logout user
    builder.addCase(logoutAsyncUser.fulfilled, (state) => {
      return {
        ...state,
        user: null,
        loading: false,
        error: null,
      };
    });
    builder.addCase(logoutAsyncUser.rejected, (state, action) => {
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
