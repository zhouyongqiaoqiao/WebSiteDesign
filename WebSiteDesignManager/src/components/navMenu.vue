
<!-- 
这个组件是用来封装左边菜单的部分 --> 
<template>
  <el-aside width="200px" class="aside">
    <el-menu :default-active="route.path" exact class="el-menu-vertical-demo" router>
      <el-menu-item class="menu" index="/index" style="padding: 0px;">
        <div style="width: 80%;padding-bottom: 30px;height: 80%; padding-left: 10px;">
          <el-image id="myImage" :src="zhaoxiImage" />
        </div>
      </el-menu-item>
      <NavItem v-for="v in items" :key="v.url" :item="v" :basePath="v.url" />
    </el-menu>
  </el-aside>
</template>


<script setup>

import { reactive } from 'vue'
import { useRouter, useRoute } from "vue-router";  //导入路由
import NavItem from './navItem.vue'  //子组件  
import zhaoxiImage from '../assets/images/朝夕智慧工厂.png'


const route = useRoute();


const items = reactive([
  {
    text: '首页',
    url: '/home/index',
    icon: 'HomeFilled'
  },
  {
    text: '系统管理',
    url: '/system',
    icon: 'Grid',
    child: [
      { text: '系统日志', url: '/system/log', icon: 'Setting' }
    ]
  },
  {
    text: '用户管理',
    url: '/user',
    icon: 'Avatar',
    child: [
      { text: '用户管理', url: '/user/index', icon: 'User' }
    ]
  },
  {
    text: '网站管理',
    url: '',
    icon: 'StarFilled',
    child: [
      { text: '主题分类', url: '/theme/index', icon: 'User' },
      { text: '网站管理', url: '/website/index', icon: 'More' },
    ]
  }, 
  {
    text: '统计分析',
    url: '/analyse/info',
    icon: 'VideoCameraFilled'
  }
])

</script>