import {jwtServerInstance} from "../index";
import qs from	'qs'

//=====================角色管理接口开始========================
//角色列表查询接口
export const getRoles = (roleLike) => {
  return jwtServerInstance.request({
    url:'/sysrole/query',
    method:'post',
    data:qs.stringify({
      roleLike:roleLike
    })
  })
}

//更新sys_role的一条数据记录
export const updateRole = (roleForm) => {
  return jwtServerInstance.request({
    url:'/sysrole/update',
    method:'post',
    data:roleForm
  })
}

//新增一条sys_role数据记录
export const addRole = (roleForm) => {
  return jwtServerInstance.request({
    url:'/sysrole/add',
    method:'post',
    data:roleForm
  })
}

//删除一条sys_role数据记录
export const deleteRole = (roleId) => {
  return jwtServerInstance.request({
    url:'/sysrole/delete',
    method:'post',
    data:qs.stringify({
      roleId:roleId
    })
  })
}


//更新角色的禁用状态
export const changeRoleStatus = (roleId,status) => {
  return jwtServerInstance.request({
    url:'/sysrole/status/change',
    method:'post',
    data:qs.stringify({
      roleId,
      status
    })
  })
}

//获取被勾选的角色和所有角色列表
export const getCheckedRoles = (userId) => {
  return jwtServerInstance.request({
    url:'/sysrole/checkedroles',
    method:'post',
    data:qs.stringify({
      userId:userId
    })
  })
}
//保存用户角色(实际是用户管理的功能)
export const saveCheckedUserRoles = (userId,checkedIds) => {
  return jwtServerInstance.request({
    url:'/sysrole/savekeys',
    method:'post',
    data:{
      userId:userId,
      checkedIds:checkedIds
    }
  })
}