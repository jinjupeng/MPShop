import Login from '@/views/Login.vue'
import FirstPage from '@/views/system/FirstPage.vue'

//菜单项级别的前端路由
const menuRouter = [
    {path:"",redirect: 'firstpage' },
    {
        name: 'firstpage',
        path: 'firstpage',
        component: FirstPage
    },
    {
        name: 'sysuser',
        path: 'sysuser',
        component: () => import('@/views/system/SystemUser.vue')
    },
    {
        name: 'sysrole',
        path: 'sysrole',
        component: () => import('@/views/system/SystemRole.vue')
    },
    {
        name: 'sysorg',
        path: 'sysorg',
        component: () => import('@/views/system/SystemOrg.vue')
    },
    {
      name: 'sysmenu',
      path: 'sysmenu',
      component: () => import('@/views/system/SystemMenu.vue')
    },
    {
      name: 'sysapi',
      path: 'sysapi',
      component: () => import('@/views/system/SystemApi.vue')
    },
    {
      name: 'sysconfig',
      path: 'sysconfig',
      component: () => import('@/views/system/SystemConfig.vue')
    },
    {
      name: 'sysdict',
      path: 'sysdict',
      component: () => import('@/views/system/SystemDict.vue')
    },
    {
        name: 'personal',
        path: 'personal',
        component: () => import('@/views/system/PersonalCenter.vue')
    },
];

export default [
  {
      path: '/',
      name: 'login',
      component: Login
  },
  {
    path: '/home',
    //name: 'home',
    component: () => import( '@/views/Home.vue'),
    children:[
        ...menuRouter
    ]
  }
]