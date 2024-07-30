<template>
    <div class="container">
        <div class="login">
            <div class="item">
                <h2>毕设网站管理系统</h2>
            </div>
            <div class="item">
                <span>账号：</span>
                <div>
                    <el-input size="small" v-model="userID" placeholder="请输入账号" />
                </div>
            </div>
            <div class="item">
                <span>账号：</span>
                <div>
                    <el-input size="small" show-password v-model="password" placeholder="请输入密码" />
                </div>
            </div>
            <div class="item">
                <span>记住我</span>
                <div>
                    <el-checkbox v-model="checkMe" size="large" />
                </div>
            </div>
            <div class="item">
                <span></span>
                <el-button size="default" type="primary" @click="login">登录</el-button>
                <el-button size="default">取消</el-button>
            </div>
        </div>
    </div>
</template>

<script>
import { reactive, toRefs } from 'vue'
import { adminLogin } from "../api/user"
import router from "../router/index"
// import md5 from "md5"
// import { $alert, $confirm, $msg_i } from "../utils/msg"
import { setToken } from '../common/apiRequest'
import { $msg_e ,$msg_s,$msg_w} from "../common/msg";
export default {
    name: "login",
    setup() {
        const loginData = reactive({
            userID: "admin",
            password: "aa123",
            checkMe: false
        });
        let login = async () => {
            let params = {
                account: loginData.userID,
                password: loginData.password,
                // password: md5(loginData.password),
                // roleId:0,
                // state:true,
                // remarks:"",
                // createTime:new Date()
            }
             let data = await adminLogin(loginData.userID,loginData.password);
             if (data.status) {
                setToken(data.token);
                $msg_s("登录成功");
                router.push("/home/index");
            }else{
                $msg_w("用户名密码不正确！");
            }
        }
        //返回数据
        return {
            ...toRefs(loginData),
            login
        }
    }
}
</script>

<style lang="scss" scoped>

.container {
    width: 100vw;
    height: 100vh;
    background: linear-gradient(to bottom, rgb(18, 55, 88), lightblue);
    display: flex;
    justify-content: center;
    align-items: center;

    .login {
        width: 350px;
        height: 200px;
        border: 1px solid white;
        border-radius: 6px;
        color: white;
        padding: 15px 20px;

        .item {
            font-size: 14px;
            display: flex;
            align-items: center;
            margin: 10px;

            span {
                width: 60px;
                text-align: right;
                padding-right: 5px;
            }

            h2 {
                flex: 1;
                text-align: center;
            }

            div {
                flex: 1;
                padding-right: 10px;
            }
        }
    }

}
</style>