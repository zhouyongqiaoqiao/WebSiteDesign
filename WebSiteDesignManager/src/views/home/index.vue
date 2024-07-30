 
<template>
 <div>我是首页</div>
</template>
 

<script setup>

import { ref, reactive, onMounted } from 'vue'

import { useRouter } from "vue-router";  //导入路由
import {
    ChromeFilled,
    Opportunity,
    SwitchFilled,
    TrendCharts,
    VideoCameraFilled
} from '@element-plus/icons-vue'

// import axios from "axios"; 
import service from "../../common/apiRequest";


const lists = ref([
    { title: '设备运行', icon: ChromeFilled, url: "/user/index" },
    { title: '产品质检', icon: Opportunity, url: "/productoutbound/info" },
    { title: '物料详情', icon: SwitchFilled, url: "/material/index" },
    { title: '考勤分析', icon: TrendCharts, url: "/attendance/analyse" },
    { title: '统计分析', icon: VideoCameraFilled, url: "/analyse/info" }
])

//材料--数据源来自于api
const materialData = reactive({
    title: "",
    chartData: [],
    chartxAxis: [],
    color: "",
    chartType: ""
});

//产品出货
const productSales = reactive({
    title: "",
    chartData: [],
    chartxAxis: [],
    color: "",
    chartType: ""
});

const production = ref({
    title: "",
    chartData: [],
    chartxAxis: [],
    color: "",
    chartType: ""
});


//生命周期 挂载完成  
onMounted(async () => {
    materialDataQuery();
    productSalesQuery();
    productionQuery();
});

//查询功能--材料入库
let materialDataQuery = async () => {

    materialData.title = "物料入库";
    materialData.chartData = [130, 210, 350, 300, 150, 430, 630, 380];
    materialData.chartxAxis = ['k311', 'g002', 'k453', 'a124', 'c433', 'd232', 'g001', 'g301'];
    materialData.color = 'rgb(236, 125, 86)';
    materialData.chartType = "bar";


    // var url = `/api/Statistics/MaterialStatistics`;
    // let reponse = await service.get(url);
    // let { success, message, data, oValue } = reponse.data;
    // if (success == true) {
    //     materialData.value = data;
    // }
    // else {
    //     alert(message);
    // }
}


//查询功能--产品销售
let productSalesQuery = async () => {

    productSales.title = "产品销售";
    productSales.chartData = [1200, 1800, 1500, 2000, 1580, 580, 2100, 1600, 1850, 1450, 1320];
    productSales.chartxAxis = ['8:00', '9:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00'];
    productSales.color = 'rgb(255, 99, 132)';
    productSales.chartType = "line";


    // var url = `/api/Statistics/ProductSales`;
    // let reponse = await service.get(url);
    // let { success, message, data, oValue } = reponse.data;
    // if (success == true) {
    //     productSales.value = data;
    // }
    // else {
    //     alert(message);
    // }
}


//查询功能--产品产量
let productionQuery = async () => {
    //启发axios 请求
    production.value = {
        title: "商品产量",
        chartData: [1000, 1100, 800, 780, 850, 1500, 1670, 1275, 900, 600, 780, 850, 1100, 1240],
        chartxAxis: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        color: "#ea8052",
        chartType: "line"
    };

}

const router =useRouter();
const goto = (url) => {
    router.push({ path: url });
}

</script>