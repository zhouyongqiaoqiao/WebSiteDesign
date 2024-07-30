<!-- UserForm.vue -->
<template>
  <el-form :model="user" :rules="rules" ref="form" label-position="left" label-width="100px">
    <el-form-item label="用户编号" prop="userid">
      <el-input v-model="user.userid" disabled="!isAdd"></el-input>
    </el-form-item>
    <el-form-item label="用户名称" prop="username">
      <el-input v-model="user.username"></el-input>
    </el-form-item>
    <el-form-item label="性别">
      <el-select v-model="user.sex" placeholder="请选择">
        <el-option label="男" key="1" value="1" />
        <el-option label="女" key="0" value="0" />
      </el-select>
    </el-form-item>
    <el-form-item label="身份证" prop="idNumber">
      <el-input v-model="user.idNumber"></el-input>
    </el-form-item>
    <el-form-item label="密码" prop="password" v-if="isAdd">
      <el-input type="password" v-model="user.password"></el-input>
    </el-form-item>

    <el-form-item>
      <el-button type="primary" @click="submitForm">提交</el-button>
      <el-button @click="resetForm" v-if="isAdd">重置</el-button>
    </el-form-item>
  </el-form>
</template>

<script setup>
import { defineProps, reactive, ref, defineEmits, onMounted } from "vue";
import { createUser, updateUser } from "../../api/user";
import { $msg_e, $msg_s, $msg_w } from "../../common/msg";

const emit = defineEmits(["handleCompleted"]);
// 表单数据模型
const form = defineProps({
  isAdd: {
    type: Boolean,
    default: true,
    required: true,
  },
  userid: {
    type: String,
    default: "",
    required: true,
  },
  username: {
    type: String,
    default: "",
    required: true,
  },
  idNumber: {
    type: String,
    default: "",
    required: true,
  },
  password: {
    type: String,
    default: "",
    required: false,
  },
  sex: {
    type: String,
    default: "1",
    required: true,
  },
});

const data = defineProps({
  modelValue: {
    type: Object,
    default: () => {
      return {
        userid: "",
        username: "",
        idNumber: "",
        password: "",
        sex: "1",
      };
    },
  },
});

let user = reactive({
  userid: form.userid,
  username: form.username,
  idNumber: form.idNumber,
  password: form.password,
  sex: form.sex,
});

onMounted(() => {
  console.log("userForm mounted");
  //延时1秒触发事件
  setTimeout(() => {
    emit("onMountedCompleted");
    console.log("userForm mounted after 1 second");
  }, 1000);
});

// 表单验证规则
const rules = {
  userid: [{ required: true, message: "请输入用户编号", trigger: "blur" }],
  username: [{ required: true, message: "请输入用户名", trigger: "blur" }],
  password: [
    { required: true, message: "请输入密码", trigger: "blur" },
    { min: 6, message: "密码至少为 6 位", trigger: "blur" },
  ],
  idNumber: [{ required: false, message: "请输入身份证号码", trigger: "blur" }],
};

// 验证确认密码与密码是否一致
function validateConfirmPassword(rule, value, callback) {
  if (value !== form.password) {
    callback(new Error("两次输入的密码不一致"));
  } else {
    callback();
  }
}

// 提交表单方法
const submitForm = async () => {
  // 这里可以添加表单提交逻辑，例如调用 API 发送数据至后端服务器
  console.log("Submit:", user);
  let params = { Id: user.userid, UserName: user.username, IdNumber: user.idNumber, PassWord: user.password, SexType: user.sex };
  let res;
  if (form.isAdd) {
    res = await createUser(params);
    $msg_s("添加成功");
  } else {
    res = await updateUser(params);
    $msg_s("修改成功");
  }
  emit("handleCompleted", res);
  console.log("res:", res);
};

const initUser = (userModel) => {
  user.userid = userModel.userid;
  user.username = userModel.username;
  user.idNumber = userModel.idNumber;
  user.password = userModel.password;
  user.sex = userModel.sex;
};

// 重置表单方法
const resetForm = () => {
  user.username = "";
  user.userid = "";
  user.password = "";
  user.idNumber = "";
  // 清除表单验证错误
  // (this.$refs.userForm as any).clearValidate();
};
</script>
