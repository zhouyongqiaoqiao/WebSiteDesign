// import { $get,$post,$put,$delete ,getDataResult} from "../common/apiRequest"

import axios from "axios";
// import nProgress from "nprogress";
import { BASE_URL, AuthorServer } from "../common/index";
import { $get,$post,$put,$delete ,getDataResult} from "../common/apiRequest"

// service: 创建好的 axios 封装
const service = axios.create({
  baseURL: AuthorServer,
  setTimeout: 30000,
});

//登录， 若成功或得到token，失败返回""
export const adminLogin = async (userid, password) => {
  try {
    var res = await service.get(`/User/Login/${userid}/${password}`);
    console.log(res);
    return res.data;
  } catch (error) {
    console.log(error);
  }
  // let res =  await service.get(`/login?userID=${userid}&pwd=${password}`);

  return res;
};

export const getUserInfo = async (userid) => {
  try {
    var res = await service.get(`/User/GetUserInfo/${userid}`);
    console.log(res);
    return res.data;
  } catch (error) {
    console.log(error);
  }
}
  // let res =  await service.get(`/login?userID=${userid}&pwd=${password}`);

  export const createUser = async (user) => {
    try {
      var res = await $post(`/User/CreateUser2`,user);
      console.log(res);
      return res.data;
    } catch (error) {
      console.log(error);
    }
  };

  export const updateUser = async (user) => {
    try {
      const url =`/User/UpdateUser/${user.Id}/${user.UserName}/${user.IdNumber}`
      var res = await $put(url);
      console.log(res);
      return res.data;
    } catch (error) {
      console.log(error);
    }
  };

  export const deleteUser = async (userid) => {
    try {
      var res = await $delete(`/User/DeleteUser/${userid}`);
      console.log(res);
      return res.data;
    } catch (error) {
      console.log(error);
    }
  };


