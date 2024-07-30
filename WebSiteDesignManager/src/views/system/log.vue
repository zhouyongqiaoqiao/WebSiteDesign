<template>
    <el-card class="box-card">
        <el-row>
            <el-form :model="searchQuery" class="demo-form-inline" :inline="true">
                <el-form-item label="时间范围" style="color:aliceblue">
                    <div class="block">
                        <el-date-picker v-model="searchQuery.searchDateTime" type="datetimerange" range-separator="至"
                            start-placeholder="开始时间" end-placeholder="结束时间" />
                    </div>
                </el-form-item>
                <el-form-item style="color:aliceblue">
                    <el-button type="primary" @click="logQuery">查询</el-button>
                </el-form-item>
            </el-form>
        </el-row>
        <el-row>
            <el-table :data="tableData" style="width: 100%" :row-class-name="tableRowClassName">
                <el-table-column type="index" label="序号" width="60" />
                <el-table-column prop="stringDateTime" label="时间" />
                <el-table-column prop="logger" label="日志发生地址">
                    <template #default="scope">
                        <div style="display: flex; align-items: center">
                            <el-popover :width="300"
                                popper-style="box-shadow: rgb(14 18 22 / 35%) 0px 10px 38px -10px, rgb(14 18 22 / 20%) 0px 10px 20px -15px; padding: 20px;">
                                <template #reference>
                                    {{ scope.row.logger.substring(0, 20) }}
                                </template>
                                <template #default>
                                    <div class="demo-rich-conent" style="display: flex; gap: 16px; flex-direction: column">
                                        <div>
                                            {{ scope.row.logger }}
                                        </div>
                                    </div>
                                </template>
                            </el-popover>
                        </div>
                    </template>
                </el-table-column>
                <el-table-column prop="message" label="日志消息">
                    <template #default="scope">
                        <div style="display: flex; align-items: center">
                            <el-popover :width="300"
                                popper-style="box-shadow: rgb(14 18 22 / 35%) 0px 10px 38px -10px, rgb(14 18 22 / 20%) 0px 10px 20px -15px; padding: 20px;">
                                <template #reference>
                                    {{ scope.row.message.substring(0, 20) }}
                                </template>
                                <template #default>
                                    <div class="demo-rich-conent" style="display: flex; gap: 16px; flex-direction: column">
                                        <div>
                                            {{ scope.row.message }}
                                        </div>
                                    </div>
                                </template>
                            </el-popover>
                        </div>
                    </template>
                </el-table-column>
                <el-table-column prop="exception" label="异常消息">
                    <template #default="scope">
                        <div style="display: flex; align-items: center">
                            <el-popover :width="300"
                                popper-style="box-shadow: rgb(14 18 22 / 35%) 0px 10px 38px -10px, rgb(14 18 22 / 20%) 0px 10px 20px -15px; padding: 20px;">
                                <template #reference>
                                    <!-- {{ scope.row.exception }} -->
                                    {{ scope.row.exception.substring(0, 20) }}
                                </template>
                                <template #default>
                                    <div class="demo-rich-conent" style="display: flex; gap: 16px; flex-direction: column">
                                        <div>
                                            {{ scope.row.exception }}
                                        </div>
                                    </div>
                                </template>
                            </el-popover>
                        </div>
                    </template>
                </el-table-column>
            </el-table>
            <div class="pagination">
                <el-config-provider :locale="zhCn">
                    <el-pagination v-model:current-page="searchQuery.currentPage" v-model:page-size="searchQuery.pageSize"
                        :page-sizes="[10, 20, 30, 40]" :small="small" :disabled="disabled" :background="background"
                        layout="total,sizes, prev, pager, next,jumper" :total="searchQuery.recordCount"
                        @size-change="handleSizeChange" @current-change="handleCurrentChange">
                    </el-pagination>
                </el-config-provider>
            </div>
        </el-row>
    </el-card>
</template>

<script setup>
import { ref, onMounted, callWithAsyncErrorHandling } from 'vue';
// import axios from "axios"; 
import service from "../../common/apiRequest";
import systemLogApi from "../../api/systemLog"

// //中文包
import zhCn from "element-plus/lib/locale/lang/zh-cn"


//列表数据绑定结果
const tableData = ref([])
const searchQuery = ref({
    currentPage: 1,
    pageSize: 10,
    recordCount: 0,
    searchDateTime: []
})



//当pagesize的值发生改变的触发--也要触发列表的更新
const handleSizeChange = (number) => {
    logQuery();
}

//点击选中某一页的时候，触发,number 定位到第几页
const handleCurrentChange = (number) => {
    logQuery();
}



//数据应该是请求webapi 
//页面加载完毕--触发，加载数据
onMounted(async () => {
    //在这里就可以去请求api，返回数据
    // 如果要发起请求，axios  
    logQuery();
});



const logQuery = async () => {

    // var url = `/System/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}`;
    var searchDatearray = searchQuery.value.searchDateTime;
    let data;
    if (searchDatearray.length == 2) {
        var timestampStart = Date.parse(searchDatearray[0]) / 1000;
        var timestampEnd = Date.parse(searchDatearray[1]) / 1000;
        data = await systemLogApi.getSystemLogPage2(searchQuery.value.currentPage, searchQuery.value.pageSize, timestampStart, timestampEnd)
        // url = `/System/${searchQuery.value.currentPage}/${searchQuery.value.pageSize}/${timestampStart}/${timestampEnd}`;

    }
    else {
        data = await systemLogApi.getSystemLogPage(searchQuery.value.currentPage, searchQuery.value.pageSize);
    }

    console.log("已经拿到结果了~");
    tableData.value = data.dataList;
    searchQuery.value.recordCount = data.recordCount;

}

const getTimeStr = (date) => {
    let str = `${date.getFullYear()}-${pad(date.getMonth())}-${pad(date.getDate())} ${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`
    return str;
}

// 定义具体处理标准
// timeEl 传递过来具体的数值：年月日时分秒
// total 字符串总长度 默认值为2
// str 补充元素 默认值为"0"
function pad(timeEl, total = 2, str = '0') {
    return timeEl.toString().padStart(total, str)
}

</script>


<style>
.el-table .warning-row {
    --el-table-tr-bg-color: var(--el-color-warning-light-9);
}

.el-table .success-row {
    --el-table-tr-bg-color: var(--el-color-success-light-9);
}
</style>