<template>
   <!-- 无子级 -->
   <el-menu-item class="menu" :index="basePath" v-if="!item.child || item.child.length <= 0">
      <el-icon>
         <component :is="item.icon"></component>
      </el-icon>
      <span>{{ item.text }}</span>
   </el-menu-item>
   <!-- 有子级 -->
   <el-sub-menu class="menu" :index="basePath" v-else>
      <template #title>
         <el-icon>
            <component :is="item.icon"></component>
         </el-icon>
         <span>{{ item.text }}</span>
      </template>
      <NavItem v-for="sub in item.child" :key="sub.url" :item="sub" :basePath="sub.url" />
   </el-sub-menu>
</template>

<script setup>



// import { ref, reactive } from 'vue'
// import { initIcon } from '../common/index'

const { item, basePath } = defineProps({
   item: {
      type: Object,
   },
   basePath: {
      type: String
   }
})

</script> 