import { fileURLToPath, URL } from 'url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  // server: {
  //   proxy: {
  //     '/api': {
  //       // 本地环境
  //       target: "http://localhost:5029/api",
  //       // 启用跨域访问
  //       changeOrigin: true,
  //       // 修改请求路径
  //       rewrite: path => path.replace(/^\/api/, '')
  //     }
  //   }
  // }
})
