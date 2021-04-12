import {jwtServerInstance} from "../index";
import {getTokenUser} from "@/lib/utils";
import qs from	'qs'

//组织机构树形列表接口
export const getOrgTree = (orgQueryForm) => {
  return jwtServerInstance.request({
    url:'/sysorg/tree',
    method:'post',
    data:qs.stringify({
      username: getTokenUser(),
      orgNameLike:orgQueryForm.name,
      orgStatus: orgQueryForm.status
    })
  })
}

//更新组织机构的一条数据记录
export const updateOrg = (orgForm) => {
  return jwtServerInstance.request({
    url:'/sysorg/update',
    method:'post',
    data:orgForm
  })
}

//新增一条组织机构数据记录
export const addOrg = (orgForm) => {
  return jwtServerInstance.request({
    url:'/sysorg/add',
    method:'post',
    data:orgForm
  })
}

//删除一条组织机构数据记录
export const deleteOrg = (orgForm) => {
  return jwtServerInstance.request({
    url:'/sysorg/delete',
    method:'post',
    data:orgForm
  })
}


//更新组织机构的禁用状态
export const changeOrgStatus = (orgId,status) => {
  return jwtServerInstance.request({
    url:'/sysorg/status/change',
    method:'post',
    data:qs.stringify({
      orgId,
      status
    })
  })
}