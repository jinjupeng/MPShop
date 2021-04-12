import {jwtServerInstance} from "../index";
import qs from	'qs'

//=====================配置参数管理========================
//查询全局配置参数管理

export const getAllSysConfig = () => {
  return jwtServerInstance.request({
    url:'/sysconfig/all',
    method:'post'
  })
}

//刷新数据库全局配置到服务端内存
//并再次加载全局配置参数到前端
export const refreshSysConfig = () => {
  return jwtServerInstance.request({
    url:'/sysconfig/refresh',
    method:'post'
  })
}


//参数配置列表查询接口
export const getConfigs = (configLike) => {
  return jwtServerInstance.request({
    url:'/sysconfig/query',
    method:'post',
    data:qs.stringify({
      configLike:configLike
    })
  })
}

//更新sys_config的一条数据记录
export const updateConfig = (configForm) => {
  return jwtServerInstance.request({
    url:'/sysconfig/update',
    method:'post',
    data:configForm
  })
}

//新增一条sys_config数据记录
export const addConfig = (configForm) => {
  return jwtServerInstance.request({
    url:'/sysconfig/add',
    method:'post',
    data:configForm
  })
}

//删除一条sys_config数据记录
export const deleteConfig = (configId) => {
  return jwtServerInstance.request({
    url:'/sysconfig/delete',
    method:'post',
    data:qs.stringify({
      configId:configId
    })
  })
}