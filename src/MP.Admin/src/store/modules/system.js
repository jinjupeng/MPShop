import {getAllSysConfig,refreshSysConfig} from '@/api/system/sys_config'
import {getAllSysDict}  from '@/api/system/sys_dict'
import axios from 'axios'

const state = {
  sysconfig:[],
  sysdict:[]
}
const actions = {
  loadSysConfig({state}){
    return new Promise((resolve,reject) => {
      //没加载过才加载，已经加载过就不加载了
      //也就说只有登录，或者页面刷新时才重新加载全局配置
      if(state.sysconfig.length <= 0
        || state.sysdict.length <= 0){
        axios.all([getAllSysConfig(), getAllSysDict()])
        .then(axios.spread(function (res1, res2) {
          // 两个请求都执行完成后，进入该函数
          state.sysconfig = res1.data
          state.sysdict = res2.data
        }))
      }
      resolve();
    })
  },
  refreshConfig({state}){
    return new Promise((resolve,reject) => {
      refreshSysConfig().then(res => {
        state.sysconfig = res.data
        resolve();
      });
    })
  },
  refreshDict({state}){
    return new Promise((resolve,reject) => {
      getAllSysDict().then(res => {
        state.sysdict = res.data
        resolve();
      });
    })
  },
}
const mutations = {

}
const getters = {
  getSysConfigItem: (state) => (paramKey) => {
    //console.log(paramKey) // user.init.password
    return state.sysconfig
      .find(item => item.paramKey === paramKey)
      .paramValue
  },
  getSysDictName:(state) => (groupCode,itemValue) => {
    return state.sysdict
      .find(item => {
        return (item.groupCode === groupCode
        && item.itemValue === String(itemValue))
      }).itemName
  }
}

export default {
  state,actions,mutations,getters
}