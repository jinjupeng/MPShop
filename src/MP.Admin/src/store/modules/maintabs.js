import {getMenuTreeByUsername} from '../../api/system/sys_menu'

const state = {
    //存放{ route: 路由路径, name: tab显示名称}对象数组
    maintabs:[{route: "/home/firstpage", name: "首页",closable:false}],
    //当前被激活显示的那个Tab内容对应的route
    activeRoute: "/home/firstpage",
    menuList:[
        {
            name:"非菜单路由,但是需要显示Tab,请定义在这里",
            children:[
                {name:"个人中心",path:"/home/personal"},
            ]
        }/*,
        {   id:1,
            name:"系统管理",
            path:"/system",
            icon:"el-icon-lock",
            children:[
                {id:3,name:"用户管理",path:"/home/sysuser"},
                {id:4,name:"角色管理",path:"/home/sysrole"},
                {id:6,name:"组织管理",path:"/home/sysorg"},
                {id:7,name:"菜单管理",path:"/home/sysmenu"},
                {id:8,name:"接口管理",path:"/home/sysapi"},
            ]
        },
        {   id:2,
            name:"订单管理",
            path:"/order",
            icon:"el-icon-eleme",
            children:[
                {id:5,name:"订单详情",path:"/home/order"},
            ]
        }*/
    ],
    addTabName:""
}
const actions = {
  addTab({state,commit},route){
    getMenuTreeByUsername().then(res => {
        state.menuList = [state.menuList[0],...res.data]
        commit("addTabMutation",route);
    })
  }
}
const mutations = {
    addTabMutation(state, route) {
        let isAlreadyIn =
            state.maintabs.some(item => item.route === route)
        this.commit("findMenuNameByRoute",route);
        state.activeRoute = route;
        if(!isAlreadyIn && state.addTabName !== ""){
            state.maintabs.push({route:route,name:state.addTabName});
        }
    },
    removeTab(state, route){
        if(route !== "/home/firstpage"){
            state.maintabs = state.maintabs.filter(
                item => item.route !== route
            )
            state.activeRoute = state.maintabs[state.maintabs.length-1].route
        }
    },
    findMenuNameByRoute(state, route){
        let findOne;
        for(let i in state.menuList){
            let tmpArr = state.menuList[i].children.filter(
                item => item.path === route
            )
            if(tmpArr.length > 0) {
                findOne = tmpArr[0]
                break;
            }
        }
        state.addTabName = findOne?findOne.name:"";
    }
}
const getters = {

}

export default {
    state,actions,mutations,getters
}