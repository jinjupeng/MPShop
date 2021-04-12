import {jwtServerInstance} from "../index";
import qs from	'qs'

//=====================接口管理接口开始========================
//完整接口树形列表接口
export const getApiTree = (apiQueryForm) => {
  return jwtServerInstance.request({
    url:'/sysapi/tree',
    method:'post',
    data:qs.stringify({
      apiNameLike:apiQueryForm.name,
      apiStatus: apiQueryForm.status
    })
  })
}

//更新sys_api的一条数据记录
export const updateApi = (apiForm) => {
  return jwtServerInstance.request({
    url:'/sysapi/update',
    method:'post',
    data:apiForm
  })
}

//新增一条sys_api数据记录
export const addApi = (apiForm) => {
  return jwtServerInstance.request({
    url:'/sysapi/add',
    method:'post',
    data:apiForm
  })
}

//删除一条sys_api数据记录
export const deleteApi = (apiForm) => {
  return jwtServerInstance.request({
    url:'/sysapi/delete',
    method:'post',
    data:apiForm
  })
}

//更新api接口的禁用状态
export const changeApiStatus = (apiId,status) => {
  return jwtServerInstance.request({
    url:'/sysapi/status/change',
    method:'post',
    data:qs.stringify({
      apiId,
      status
    })
  })
}

//获取api树形结构数据（带勾选节点和默认展开节点）
export const getApiCheckedTree = (roleId) => {
  return jwtServerInstance.request({
    url:'/sysapi/checkedtree',
    method:'post',
    data:qs.stringify({
      roleId:roleId
    })
  })
}

//保存api树勾选节点(checkedId是一个数字型数组)
export const saveApiCheckedKeys = (roleId,checkedIds) => {
  return jwtServerInstance.request({
    url:'/sysapi/savekeys',
    method:'post',
    data:{
      roleId:roleId,
      checkedIds:checkedIds
    }
  })
}