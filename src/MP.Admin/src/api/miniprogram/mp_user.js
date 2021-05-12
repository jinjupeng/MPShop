import  { jwtServerInstance }  from '../index'
import qs from	'qs'

//=====================微信用户管理接口开始========================
//用户列表查询接口
export const getUsers = (queryform,pagination) => {
  return jwtServerInstance.request({
    url:'/mpuser/pc/mpuser/query',
    method:'post',
    data:{
      nickname: queryform.nickname,
      createStartTime: queryform.timeRange[0],
      createEndTime: queryform.timeRange[1],
      pageNum: pagination.pageNum,
      pageSize: pagination.pageSize
    }
  })
}


//删除一条mp_user数据记录
export const deleteUser = (userId) => {
  return jwtServerInstance.request({
    url:'/mpuser/pc/mpuser/delete',
    method:'post',
    data:qs.stringify({
      userId:userId
    })
  })
}


