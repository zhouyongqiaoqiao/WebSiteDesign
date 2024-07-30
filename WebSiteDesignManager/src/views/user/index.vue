
<template>
    <el-card class="box-card">
        <el-row>
            <el-form class="demo-form-inline" :inline="true">
                <el-form-item label="关键字" style="color:aliceblue">
                    <div class="block">
                        <el-input v-model="searchQuery.searchString" placeholder="姓名,身份证号码" />
                    </div>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="userQuery">查询</el-button>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="clickAddUser()">新增</el-button>
                </el-form-item>
            </el-form>
        </el-row>
        <el-row>
            <el-table :data="tableData" row-key="id" style="width: 100%" >
                <el-table-column type="index" label="序号" width="60" />
                <el-table-column prop="userName" label="员工名称" />
                <!-- <el-table-column prop="age" label="员工年龄" /> -->
                <el-table-column prop="sexTypeDescription" label="性别">
                    <!-- <template #default="scope">
                        {{ getSexType(scope.row.sexType) }}
                    </template> -->
                </el-table-column>
                <el-table-column prop="jobStatusDescription" label="在职状态" />
                <el-table-column label="操作" width="500">
                    <template #default="item">
                        <el-row class="mb-12">
                            <el-button size="small" @click="Show1(1)" type="success">查看考情</el-button>
                            <el-button size="small" @click="Show1(2)" type="info">查看工作日志</el-button>
                            <el-button size="small" @click="clickUpdateUser(item.row)" type="warning">编辑员工</el-button>
                            <el-button size="small" @click="clickDeleteUser(item.row)" type="danger">删除员工</el-button>
                        </el-row>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <el-config-provider :locale="zhCn">
                    <el-pagination v-model:current-page="searchQuery.currentPage" v-model:page-size="searchQuery.pageSize"
                        :page-sizes="[10, 20, 30, 40]" small="small" 
                        layout="total,sizes, prev, pager, next,jumper" :total="searchQuery.recordCount"
                        @size-change="handleSizeChange" @current-change="handleCurrentChange">
                    </el-pagination>
                </el-config-provider>
            </div>
        </el-row>
    </el-card>

    <el-drawer ref="drawerRef" v-model="dialog" :title="formTitle" direction="rtl"
        class="demo-drawer">
        <UserForm ref="userFormRef" 
        @handleCompleted="userHandleCompleted" 
        @onMountedCompleted="onChildMountedCompleted" 
        v-model:isAdd="isAddUser"
        v-model:user="formData" >
        </UserForm>  
    </el-drawer>
</template>

<script setup>

import { ref, reactive, onMounted, callWithAsyncErrorHandling } from 'vue';
// import axios from "axios"; 
import service from "../../common/apiRequest";
// //中文包
import zhCn from "element-plus/lib/locale/lang/zh-cn"
import UserForm from './userForm.vue'
import { $msg_s, $msg_w, $msg_e, $msg_i, $alert, $confirm } from "../../common/msg";
import { deleteUser } from "../../api/user";
//列表数据绑定结果
const tableData = ref([])
const searchQuery = ref({
    currentPage: 1,
    pageSize: 10,
    recordCount: 0,
    searchString: ""
})

let formTitle = ref("新增员工")
//子组件参数
const formData = reactive({
    userId: "",
    userName: "",
    sex: "0",
    idNumber: ""
})

let isAddUser = ref(true);
let userFormRef =ref(); //子组件 初始化

//数据应该是请求webapi 
//页面加载完毕--触发，加载数据
onMounted(async () => {
    //在这里就可以去请求api，返回数据
    // 如果要发起请求，axios  
    userQuery();
});

let dialog = ref(false)
let openDrawer = (isAdd) => {
    isAddUser.value = isAdd;
    formTitle = isAddUser.value ? "新增员工" : "编辑员工";
    console.log(111,formData,userFormRef.value)
    dialog.value = true;
    // userFormRef.value.initUser(formData);
    console.log("222",formData,userFormRef.value)
    console.log("isAdd",isAdd)
}

//查询功能
let userQuery = async () => {

    var url = `/User/GetUserPage/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}`;
    var searchString = searchQuery.value.searchString;
    if (searchString) {
        url = `/User/GetUserPage/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}/${searchString}`;
    }
    let reponse = await service.get(url);
    let { success, message, data, oValue } = reponse.data;

    console.log("已经拿到结果了~");
    if (success == true) {
        tableData.value = data.dataList;
        searchQuery.value.recordCount = data.recordCount;
    }
    else {
        alert(message);
    }
}


//当pagesize的值发生改变的触发--也要触发列表的更新
const handleSizeChange = (number) => {
    userQuery();
}

//点击选中某一页的时候，触发,number 定位到第几页
const handleCurrentChange = (number) => {
    userQuery();
}

const userHandleCompleted = (user) => {
    dialog.value = false;
    userQuery();
}

const onChildMountedCompleted = () => {
    // userFormRef.value.initUser(formData);
}

const clickAddUser = () => {
    formData.userId = "";
    formData.userName = "";
    formData.sex = "1";
    formData.idNumber = "";
    formData.password = "";
    isAddUser.value = true;
    openDrawer(true);
}

const clickUpdateUser = (user) => {
    formData.userId = user.id;
    formData.userName = user.userName;
    formData.sex = user.sexType.toString();
    formData.idNumber = user.idNumber;
    formData.password = user.password;
    isAddUser.value = false;
    openDrawer(false);
}

const clickDeleteUser = (user) => {

    $confirm(`确认删除${user.userName}(${user.id})吗？`, async () => {
        let res = await deleteUser(user.id);
        if (res) {
            userQuery();
        }
        else {
            $msg_e(res.data.message);
        }

    }, () => { },);
}

const getSexType = (sexType) => {
    if (sexType == 1) {
        return "男性";
    }
    else {
        return "女性";
    }
}


const Show1 = (type) => {
    alert(type + "正在建设中~~");
}

</script>