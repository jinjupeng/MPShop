import  { jwtServerInstance }  from '../index'
import {getTokenUser} from "@/lib/utils";
import qs from	'qs'

//JWT登录认证接口
export const login = (userName,passWord) => {
  return jwtServerInstance.request({
    url:'/JwtAuth/authentication',
    method:'post',
    data:{
      username: userName,
      password: passWord
    }
  })
}
//JWT令牌刷新接口
export const refreshToken = () => {
  return jwtServerInstance.request({
    url:'/JwtAuth/refreshtoken',
    method:'get'
  })
}

//获取用户信息接口(个人中心)
export const getUserInfo = () => {
  return jwtServerInstance.request({
    url:'/sysuser/info',
    method:'get',
    params:{
      username: getTokenUser()
    }
  })
}


//=====================用户管理接口开始========================
//用户列表查询接口
export const getUsers = (queryform,pagination) => {
  return jwtServerInstance.request({
    url:'/sysuser/query',
    method:'post',
    data:qs.stringify({
      orgId: queryform.orgId,
      username: queryform.username,
      phone: queryform.phone,
      email: queryform.email,
      enabled: queryform.enabled,
      createStartTime: queryform.timeRange[0],
      createEndTime: queryform.timeRange[1],
      pageNum: pagination.pageNum,
      pageSize: pagination.pageSize
    })
  })
}

//更新sys_user的一条数据记录
export const updateUser = (userForm) => {
  return jwtServerInstance.request({
    url:'/sysuser/update',
    method:'post',
    data:userForm
  })
}

//新增一条sys_user数据记录
export const addUser = (userForm) => {
  return jwtServerInstance.request({
    url:'/sysuser/add',
    method:'post',
    data:userForm
  })
}

//删除一条sys_user数据记录
export const deleteUser = (userId) => {
  return jwtServerInstance.request({
    url:'/sysuser/delete',
    method:'post',
    data:qs.stringify({
      userId:userId
    })
  })
}

//sys_user数据记录的enabled字段
export const changeEnabled = (userId,enabled) => {
  return jwtServerInstance.request({
    url:'/sysuser/enabled/change',
    method:'post',
    data:qs.stringify({
      userId,
      enabled
    })
  })
}

//重置用户密码
export const resetUserPwd = (userId) => {
  return jwtServerInstance.request({
    url:'/sysuser/pwd/reset',
    method:'post',
    data:qs.stringify({
      userId:userId
    })
  })
}

//判断当前用户是否仍在使用默认密码，没做修改
export const pwdIsDefault = () => {
  return jwtServerInstance.request({
    url:'/sysuser/pwd/isdefault',
    method:'post',
    data:qs.stringify({
      username:getTokenUser()
    })
  })
}


//修改密码
export const changeUserPwd = (oldPass,newPass) => {
  return jwtServerInstance.request({
    url:'/sysuser/pwd/change',
    method:'post',
    data:qs.stringify({
      username:getTokenUser(),
      oldPass:oldPass,
      newPass:newPass
    })
  })
}



