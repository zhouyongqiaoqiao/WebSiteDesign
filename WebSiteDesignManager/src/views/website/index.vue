
<template>
    <el-card class="box-card">
        <el-row>
            <el-form class="demo-form-inline" :inline="true">
                <el-form-item label="关键字" style="color:aliceblue">
                    <div class="block">
                        <el-input v-model="searchQuery.searchString" placeholder="名称,编号" />
                    </div>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="defaultQuery">查询</el-button>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="clickAddWebSite()">新增</el-button>
                </el-form-item>
            </el-form>
        </el-row>
        <el-row>
            <el-table :data="tableData" row-key="id" style="width: 100%">
                <el-table-column type="index" label="序号" width="60" />
                <el-table-column label="主图" width="60">
                    <template #default="item">
                        <el-image :src="item.row.mainImageUrl" style="width: 60px;height: 60px;" fit="contain">
                                <template #error>
                                <div class="image-slot">
                                    <el-icon><icon-picture /></el-icon>
                                </div>
                                </template>
                        </el-image>
                    </template>
                </el-table-column>
                <el-table-column prop="name" label="名称" />
                <el-table-column prop="theme" label="主题" />
                <el-table-column prop="pageCount" label="页数" />
                <el-table-column prop="price" label="价格" />
                <el-table-column label="操作" width="500">
                    <template #default="item">
                        <el-row class="mb-12">
                            <el-button size="small" @click="clickUpdateWebSite(item.row)" type="warning">编辑</el-button>
                            <el-button size="small" @click="clickStopUseWebsite(item.row)" type="danger">停用</el-button>
                        </el-row>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <el-config-provider :locale="zhCn">
                    <el-pagination v-model:current-page="searchQuery.currentPage" v-model:page-size="searchQuery.pageSize"
                        :page-sizes="[10, 20, 30, 40]" small="small" layout="total,sizes, prev, pager, next,jumper"
                        :total="searchQuery.recordCount" @size-change="handleSizeChange"
                        @current-change="handleCurrentChange">
                    </el-pagination>
                </el-config-provider>
            </div>
        </el-row>
    </el-card>

    <el-drawer  size="80%" ref="drawerRef" v-model="dialog" :title="formTitle" direction="rtl" class="demo-drawer">
        <websiteForm ref="websiteFormRef" @handleCompleted="userHandleCompleted"
            @onMountedCompleted="onChildMountedCompleted" v-model:isAdd="isAdd" v-model:website="formData">
        </websiteForm>
    </el-drawer>
</template>

<script setup>

import { ref, reactive, onMounted, callWithAsyncErrorHandling } from 'vue';
// import axios from "axios"; 
import service from "../../common/apiRequest";
// //中文包
import zhCn from "element-plus/lib/locale/lang/zh-cn"
import websiteForm from './websiteForm.vue'
import { $msg_s, $msg_w, $msg_e, $msg_i, $alert, $confirm } from "../../common/msg";
import { stopUseWebSite } from "../../api/website";
//列表数据绑定结果
const tableData = ref([])
const searchQuery = ref({
    currentPage: 1,
    pageSize: 10,
    recordCount: 0,
    searchString: ""
})

let formTitle = ref("新增网站")
//子组件参数
const formData = reactive({
    id: "",
    name: "",
    theme: "",
    pageCount: 0,
    price: 0,
    description: "",
    sortNo: 10,
    mainImageUrl: ""
})

let isAdd = ref(true);
let websiteFormRef = ref(); //子组件 初始化

//数据应该是请求webapi 
//页面加载完毕--触发，加载数据
onMounted(async () => {
    //在这里就可以去请求api，返回数据
    // 如果要发起请求，axios  
    defaultQuery();
});

let dialog = ref(false)
let openDrawer = (isAddtheme) => {
    isAdd.value = isAddtheme;
    formTitle = isAdd.value ? "新增网站" : "编辑网站";
    console.log(111, formData, websiteFormRef.value)
    dialog.value = true;
    // userFormRef.value.initUser(formData);
    console.log("222", formData, websiteFormRef.value)
    console.log("isAdd", isAdd)
}

//查询功能
let defaultQuery = async () => {

    var searchString = searchQuery.value.searchString;
    let url = `/WebSite/GetWebSitePage/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}/${searchString}`;

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
    defaultQuery();
}

//点击选中某一页的时候，触发,number 定位到第几页
const handleCurrentChange = (number) => {
    defaultQuery();
}

const userHandleCompleted = (user) => {
    dialog.value = false;
    defaultQuery();
}

const onChildMountedCompleted = () => {
    // userFormRef.value.initUser(formData);
}

const clickAddWebSite = () => {
    formData.id = "";
    formData.name = "";
    formData.theme = "";
    formData.pageCount = 1;
    formData.price = 0;
    formData.description = "";
    formData.sortNo = 10;
    formData.mainImageUrl = "";
    isAdd.value = true;
    openDrawer(true);
}

const clickUpdateWebSite = (website) => {
    formData.id = website.id;
    formData.name = website.name;
    formData.theme = website.theme;
    formData.pageCount = website.pageCount;
    formData.price = website.price;
    formData.description = website.description;
    formData.sortNo = website.sortNo;
    formData.mainImageUrl = website.mainImageUrl;
    isAdd.value = false;
    openDrawer(false);
}

const clickStopUseWebsite = (website) => {

    $confirm(`确认停用${website.name}(${website.id})吗？`, async () => {
        let res = await stopUseWebSite(website.id);
        if (res) {
            defaultQuery();
        }
        else {
            $msg_e(res.data.message);
        }

    }, () => { },);
}

</script>