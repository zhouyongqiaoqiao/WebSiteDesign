

 let model ={
    dev:"http://47.116.53.148:5000/",
    dev2:"http://localhost:5084/",
    test:'http://47.116.53.148:5000/',
    pro:'http://127.0.0.1:5000/',
    authorServer:"http://47.116.53.148:5002/", //验证服务器
    authorServer2:"http://localhost:5000/" //验证服务器
}

//根路径
export const BASE_URL = model.dev2;

export const AuthorServer = model.authorServer;