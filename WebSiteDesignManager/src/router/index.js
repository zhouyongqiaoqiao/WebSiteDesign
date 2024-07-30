import { createRouter, createWebHistory } from 'vue-router'
import index from '../views/Index.vue'


//创建一个路由器对象
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'index',
      component: index,
      children: [
        {
          path: '/home/index',
          name: 'index',
          meta: { title: '首页' },
          component: () => import("../views/home/index.vue")
        },
        {
          path: '/system',
          name: 'system',
          meta: { title: '系统管理' },
          children: [
            {
              path: '/system/log',
              name: 'log',
              meta: { title: '系统日志' },
              component: () => import("../views/system/log.vue")
            }
          ]
        },
        {
          path: '/user',
          name: 'user',
          meta: { title: '用户管理' },
          children: [
            {
              path: '/user/index',
              name: 'userindex',
              meta: { title: '用户管理' },
              component: () => import("../views/user/index.vue")
            }
          ]
        },
        {
          path: '/website',
          name: 'attendance',
          meta: { title: '网站管理' },
          children: [
            {
              path: '/theme/index',
              name: 'themeindex',
              meta: { title: '主题分类' },
              component: () => import("../views/theme/index.vue")
            },
            {
              path: '/website/index',
              name: 'websiteindex',
              meta: { title: '网站管理' },
              component: () => import("../views/website/index.vue")
            }
          ]
        },      
        {
          path: '/analyse/info',
          name: 'Promotion',
          meta: { title: '统计分析' },
          component: () => import("../views/analyse/index.vue")
        }

      ]
    },
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/Login.vue'),
  },

  ]
})

//导入Nprogress
import Nprogress from 'nprogress'
import 'nprogress/nprogress.css'

//路由前置守卫
router.beforeEach((to, from, next)=> {
  // next();
  Nprogress.start();
  let token = localStorage.getItem("token")
  if(token || to.path=='/login')
  {
      next();
  }else{
      next("/login");
  }
});

//路由后置守卫
router.afterEach((to,from)=>{
   Nprogress.done();
  
});

export default router
