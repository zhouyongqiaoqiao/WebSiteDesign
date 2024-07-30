//在这里就可以封装axios 组件，封装好了以后， 其他地方直接引入使用。

import axios from "axios";
import nProgress from "nprogress";
import { BASE_URL, AuthorServer } from "../common/index";
import { callWithAsyncErrorHandling } from "vue";

//service: 创建好的 axios 封装
const service = axios.create({
  baseURL: BASE_URL,
  setTimeout: 30000,
});

//请求拦截器---在axios 发起请求之前要做的事儿   aop思想~~
service.interceptors.request.use((config) => {
  nProgress.start();
  ///在发送请求前做点什么
  console.log("请求前完成~~----AOP思想");
  // let token = mainStore().accessToken;
  // config.headers.Authorization ="Bearer "+token;

  return config;
});

//响应拦截器
service.interceptors.response.use(
  async (response) => {
    nProgress.done();
    console.log("请求已经响应-----AOP思想~~");

    return response;
  },
  (err) => {
    nProgress.done();
    console.log(err);
    return Promise.reject(err);
  }
);

//定义一个get请求
export const $get = async (url, params) => {
  let { data } = await service.get(url, params);
  return data;
};

//定义一个post请求
export const $post = async (url, params) => {
  let { data } = await service.post(url, params);
  console.log(data);
  if (!data.success) {
    console.log(data.message);
  }
  return data;
};

export const $delete = async (url, params) => {
  return (await service.delete(url, params)).data;
};

export const $put = async (url, params) => {
  return (await service.put(url, params)).data;
};

//从请求的结果中获取真实结果
export const getDataResult = (res) => {
  if (res) {
    if (res.success) {
      return res.data;
    } else {
      console.log(res.message);
    }
    return res.success;
  }
  return res;
};

export const setToken = (token) => {
  localStorage.setItem("token", token);
  service.defaults.headers.common["authorization"] = "Bearer " + token;
};

export const clearToken = () => {
  localStorage.removeItem("token");
  service.defaults.headers.common["authorization"] = "";
};

export default service;
