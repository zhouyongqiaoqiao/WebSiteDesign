
<template>
    <el-card class="box-card">
        <el-row>
            <el-form class="demo-form-inline" :inline="true">
                <el-form-item label="关键字" style="color:aliceblue">
                    <div class="block">
                        <el-input v-model="searchQuery.searchString" placeholder="名称或编号" />
                    </div>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="themeQuery">查询</el-button>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="clickAddTheme()">新增</el-button>
                </el-form-item>
            </el-form>
        </el-row>
        <el-row>
            <el-table :data="tableData" row-key="id" style="width: 100%" >
                <el-table-column type="index" label="序号" width="60" />
                <el-table-column prop="name" label="名称" />               
                <el-table-column label="操作" width="500">
                    <template #default="item">
                        <el-row class="mb-12">
                            <!-- <el-button size="small" @click="clickUpdateUser(item.row)" type="warning">编辑主题</el-button> -->
                            <el-button size="small" @click="clickDeleteTheme(item.row)" type="danger">删除主题</el-button>
                        </el-row>
                    </template>
                </el-table-column>
            </el-table>
        </el-row>
    </el-card>

    <el-drawer ref="drawerRef" v-model="dialog" :title="formTitle" direction="rtl"
        class="demo-drawer">
        <themeform ref="themeFormRef" 
        @handleCompleted="handleCompleted" 
        v-model:isAdd= "isAddTheme"
        v-model:theme="formData" >
        </themeform>  
    </el-drawer>
</template>

<script setup>

import { ref, reactive, onMounted, callWithAsyncErrorHandling } from 'vue';
// import axios from "axios"; 
import service from "../../common/apiRequest";
import themeform from './themeForm.vue';
import { $msg_s, $msg_w, $msg_e, $msg_i, $alert, $confirm } from "../../common/msg";
import { getAllThemes,getThemes, deleteTheme } from "../../api/theme";
//列表数据绑定结果
const tableData = ref([])
const searchQuery = ref({
    searchString: ""
})

let formTitle = ref("新增主题")
//子组件参数
const formData = reactive({
    Id: "",
    Name: ""
})

let isAddTheme = ref(true);
let themeFormRef =ref(); //子组件 初始化

//数据应该是请求webapi 
//页面加载完毕--触发，加载数据
onMounted(async () => {
    //在这里就可以去请求api，返回数据
    // 如果要发起请求，axios  
    themeQuery();
});

let dialog = ref(false)
let openDrawer = (isAdd) => {
    isAddTheme.value = isAdd;
    formTitle = isAddTheme.value ? "新增主题" : "编辑主题";
    console.log(111,formData,themeFormRef.value)
    dialog.value = true;
    // themeFormRef.value.initUser(formData);
    console.log("222",formData,themeFormRef.value)
    console.log("isAdd",isAdd)
}

//查询功能
let themeQuery = async () => {

    var url = `/Theme/GetAllThemes`;
    var searchString = searchQuery.value.searchString;
    if (searchString) {
        url = `/Theme/GetThemes/${searchString}`;
    }
    let reponse = await service.get(url);
    let { success, message, data, oValue } = reponse.data;
    if (success == true) {
        tableData.value = data;
    }
    else {
        alert(message);
    }
}

const handleCompleted = (user) => {
    dialog.value = false;
    themeQuery();
}

const onChildMountedCompleted = () => {
    // themeFormRef.value.initUser(formData);
}

const clickAddTheme = () => {
    formData.Id = "";
    formData.Name = "";
    openDrawer(true);
}

const clickUpdateUser = (user) => {
    // formData.Id = theme.Id;
    formData.Name = theme.Name;
    openDrawer(false);
}

const clickDeleteTheme = (theme) => {

    $confirm(`确认删除${theme.name}吗？`, async () => {
        let res = await deleteTheme(theme.name);
        if (res) {
            themeQuery();
        }
        else {
            $msg_e(res.data.message);
        }

    }, () => { },);
}

</script>