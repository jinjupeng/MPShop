
import {jwtServerInstance} from "../index";
import qs from	'qs'

//查询所有数据字典项
export const getAllSysDict = () => {
    return jwtServerInstance.request({
        url:'/sysdict/all',
        method:'post'
    })
}

//参数配置列表查询接口
export const getSysDict =
           ( groupName,
            groupCode ) => {
    return jwtServerInstance.request({
        url:'/sysdict/query',
        method:'post',
        data:qs.stringify({
            groupName,
            groupCode
        })
    })
}

//更新sys_dict的一条数据记录
export const updateSysDict = (dialogForm) => {
    return jwtServerInstance.request({
        url:'/sysdict/update',
        method:'post',
        data:dialogForm
    })
}

//新增一条sys_dict数据记录
export const addSysDict = (dialogForm) => {
    return jwtServerInstance.request({
        url:'/sysdict/add',
        method:'post',
        data:dialogForm
    })
}

//删除一条sys_dict数据记录
export const deleteSysDict = (deleteId) => {
    return jwtServerInstance.request({
        url:'/sysdict/delete',
        method:'post',
        data:qs.stringify({
            id:deleteId
        })
    })
}