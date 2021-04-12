import Vue from 'vue'
import VueRouter from 'vue-router'
import systemRoutes from './modules/system'
import { refreshToken } from '@/api/system/sys_user'
import {setJwtToken} from "@/lib/utils";
import store from '@/store/index'

//导入NProgress进度条
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'


Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [...systemRoutes]
})

// 路由守卫
// to:表示将要访问的路径，from：表示从哪里来，next：是下一个要做的操作；next('/login')强制跳转login页
router.beforeEach((to,from,next) => {
  if(to.name !== 'login'){
    NProgress.start()
    refreshToken().then(res => {
      //没有获得新的token==null，
      // 表示旧的token已经失效，需要重新登录
      if(res.data == null){
        next({name: 'login'}) //去登录界面
        setJwtToken('') //清空token
      }else{//否则去你想去的界面，并把新的token保存起来
        //把全局配置加载完成再去你想去的页面
        store.dispatch('loadSysConfig').then(_ => {
          next()
        })
        setJwtToken(res.data)
      }
    })
  }else{//每次去到登录页面都刷新一下，清除vuex状态
    next()
    setJwtToken('') //清空token
  }
})
router.afterEach((to,from) => {
  if(to.name !== 'login'){
    store.dispatch('addTab',to.path)
    NProgress.done()
  }
})

export default router
