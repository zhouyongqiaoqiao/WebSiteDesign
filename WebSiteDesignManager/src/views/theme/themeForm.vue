<template>
  
    <el-form :model="theme" :rules="rules" label-position="left" label-width="100px">
      <!-- <el-form-item label="用户编号" prop="id">
        <el-input v-model="theme.id" :disabled="!props.isAdd"></el-input>
      </el-form-item> -->
      <el-form-item label="主题名称" prop="name">
        <el-input v-model="theme.name"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm">提交</el-button>
        <el-button @click="resetForm" v-if="props.isAdd">重置</el-button>
      </el-form-item>
    </el-form>
  </template>
  
  <script setup>
  import { defineProps, reactive, ref, defineEmits, onMounted } from "vue";
  import { addTheme, updateTheme } from "../../api/theme";
  import { $msg_e, $msg_s, $msg_w } from "../../common/msg";
  
  const emit = defineEmits(["handleCompleted","onMountedCompleted"]);
  // 表单数据模型 (特别注意外部使用时一定要使用对应的字段名（这里是themeindex 中 v-model:theme="formData"）)
  const props = defineProps({
    theme: {
      type: Object,
      default: () => {
        return {
          id: "",
          name: "",
        };
      },
    },
    isAdd:{
      type: Boolean,
      default: true
    }
  });
  
  onMounted(() => {
    console.log("Form mounted");
    //延时1秒触发事件
    // setTimeout(() => {
    //   emit("onMountedCompleted");
    //   console.log("userForm mounted after 1 second",props.isAdd);
    // }, 1000);
  });
  
  // 表单验证规则
  const rules = reactive({
    // id: [{ required: true, message: "请输入主题编号", trigger: "blur" }],
    name: [{ required: true, message: "请输入主题", trigger: "blur" }],
  });
  
  
  // 提交表单方法
  const submitForm = async () => {
    // 这里可以添加表单提交逻辑，例如调用 API 发送数据至后端服务器
    console.log("Submit:", props.theme);
    let params = { Id: props.theme.id, Name: props.theme.name};
    let res;
    if (props.isAdd) {
      res = await addTheme(params);
      $msg_s("添加成功");
    } else {
      res = await updateTheme(params);
      $msg_s("修改成功");
    }
    emit("handleCompleted", res);
    console.log("res:", res);
  };
  
  
  // 重置表单方法
  const resetForm = () => {
    props.theme.id = "";
    props.theme.name = "";
    // 清除表单验证错误
    // (this.$refs.userForm as any).clearValidate();
  };
  
  //父能访问resetForm,父访问通过ref
  defineExpose({
    resetForm
  })
  </script>
  