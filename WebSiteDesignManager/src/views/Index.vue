<!-- 这里是做布局 -->

<template>
  <div id="module">
    <el-container>
      <!-- 菜单栏 -->
      <navMenu />
      <el-container>
        <!-- 头信息 -->
        <el-header>
          <!-- 面包屑 -->
          <breadcrumb />
          <!-- 右上角按钮 -->
          <div class="right-top-container">
            <!-- Button examples -->
            <!-- <button class="right-top-button primary-button">
              <i class="button-icon fas fa-cog"></i>
              Settings
            </button>
            <button class="right-top-button">Help</button> -->
            <el-button class="right-top-button" @click="onClickExit" type="primary">退出</el-button>
          </div>
        </el-header>
        <!-- 内容区域 -->
        <el-main>
          <RouterView></RouterView>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script setup>
import navMenu from "../components/navMenu.vue";
import breadcrumb from "../components/breadcrumb.vue";
import { ref } from "vue";
import { Menu as IconMenu, Message, Setting, ArrowRight, Close } from "@element-plus/icons-vue";
import { $msg_s } from "../common/msg";
import {clearToken} from "../common/apiRequest"

let onClickExit = () => {
  clearToken();
  $msg_s("退出成功!");
  window.location.href = "/login";
};
</script>

<style>
/* Right top corner container */
.right-top-container {
  position: fixed;
  top: 1rem;
  right: 1rem;
  display: flex;
  flex-direction: row; /* Change to 'column' for vertical stacking */
  align-items: center;
  justify-content: flex-end;
  gap: 0.5rem; /* Adjust gap between buttons as needed */
}

/* Button styles */
.right-top-button {
  padding: 0.5rem 1rem;
  border-radius: 4px;
  font-size: 0.9rem;
  cursor: pointer;
  transition: background-color 0.2s ease-in-out;
}

.right-top-button:hover {
  background-color: #f2f2f2; /* Change to your desired hover color */
}

/* Primary button */
.primary-button {
  background-color: #007bff; /* Example primary color */
  color: white;
}

.primary-button:hover {
  background-color: #0069d9; /* Slightly darker on hover */
}

/* Icon integration (optional) */
.button-icon {
  margin-right: 0.5rem; /* Adjust as needed */
}
</style>
