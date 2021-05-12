using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Attributes;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.AuthModel;
using ApiServer.Model.Model.DataTree;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.Nodes;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.BLL.BLL
{
    public class SysApiService : ISysApiService
    {
        private readonly IBaseService<sys_api> _baseService;
        private readonly IBaseService<sys_role_api> _baseSysRoleApiService;
        private readonly IBaseService<sys_role> _baseSysRoleService;
        private readonly IMySystemService _mySystemService;

        public SysApiService(IBaseService<sys_api> baseService, IMySystemService mySystemService,
            IBaseService<sys_role_api> baseSysRoleApiService, IBaseService<sys_role> baseSysRoleService)
        {
            _baseService = baseService;
            _mySystemService = mySystemService;
            _baseSysRoleApiService = baseSysRoleApiService;
            _baseSysRoleService = baseSysRoleService;
        }

        /// <summary>
        /// 获取到所有的角色和对应的api接口
        /// </summary>
        /// <returns></returns>
        public List<PermissionItem> GetAllApiOfRole()
        {
            List<PermissionItem> permissionItems = new List<PermissionItem>();
            List<sys_role> sysRoles = _baseSysRoleService.GetModels(a => a.status == false).ToList(); // 获取所有未禁用的角色
            List<sys_api> sysApis = _baseService.GetModels(a => a.status == false).ToList(); // 获取所有未禁用的接口
            List<sys_role_api> sysRoleApis = _baseSysRoleApiService.GetModels(null).ToList();
            foreach (var sysRole in sysRoles)
            {
                foreach (var sysRoleApi in sysRoleApis)
                {
                    if (sysRole.id == sysRoleApi.role_id)
                    {
                        sys_api sysApi = sysApis.SingleOrDefault(a => a.id == sysRoleApi.api_id);
                        if (!string.IsNullOrEmpty(sysApi.url))
                        {
                            PermissionItem permissionItem = new PermissionItem
                            {
                                Url = sysApi.url,
                                Role = sysRole.role_code
                            };
                            permissionItems.Add(permissionItem);
                        }
                    }
                }
            }

            return permissionItems;
        }

        public MsgModel GetApiTreeById(string apiNameLike, bool apiStatus)
        {
            MsgModel msg = new MsgModel
            {
                isok = true,
                message = "查询成功！"
            };
            //查找level=1的API节点，即：根节点
            sys_api rootSysApi = _baseService.GetModels(s => s.level == 1).Single();
            if (rootSysApi != null)
            {
                long rootApiId = rootSysApi.id;
                List<sys_api> sysApis = _mySystemService.SelectApiTree(rootApiId, apiNameLike, apiStatus);
                List<SysApiNode> sysApiNodes = new List<SysApiNode>();
                foreach (sys_api sys_Api in sysApis)
                {
                    SysApiNode sysApiNode = sys_Api.BuildAdapter().AdaptToType<SysApiNode>();
                    sysApiNodes.Add(sysApiNode);
                }

                if (!string.IsNullOrEmpty(apiNameLike))
                {
                    //根据api名称等查询会破坏树形结构，返回平面列表
                    msg.data = sysApiNodes;
                    return msg;
                }

                //否则返回树型结构列表
                msg.data = DataTreeUtil<SysApiNode, long>.BuildTree(sysApiNodes, rootApiId);
                return msg;
            }
            return msg;
        }

        public MsgModel UpdateApi(sys_api sys_Api)
        {
            var result = _baseService.Update(sys_Api);
            return result ? MsgModel.Success("修改接口配置成功！") : MsgModel.Fail("修改接口配置失败！");
        }

        [Transaction]
        public MsgModel AddApi(sys_api sys_Api)
        {
            MsgModel msg = new MsgModel
            {
                isok = true,
                message = "新增接口配置成功！"
            };
            sys_Api.id = new Snowflake().GetId();
            SetApiIdsAndLevel(sys_Api);
            sys_Api.is_leaf = true;//新增的菜单节点都是子节点，没有下级
            sys_api parent = new sys_api
            {
                id = sys_Api.api_pid,
                is_leaf = false//更新父节点为非子节点。
            };
            _baseService.Update(parent);
            sys_Api.status = false;//设置是否禁用，新增节点默认可用
            _baseService.Add(sys_Api);
            return msg;
        }

        [Transaction]
        public MsgModel DeleteApi(sys_api sys_Api)
        {
            // 查找被删除节点的子节点
            List<sys_api> myChild = _baseService.GetModels(s => s.api_pids.Contains("[" + sys_Api.id + "]")).ToList();
            if (myChild.Count > 0)
            {
                // "不能删除含有下级API接口的节点"
            }
            //查找被删除节点的父节点
            List<sys_api> myFatherChild = _baseService.GetModels(s => s.api_pids.Contains("[" + sys_Api.api_pid + "]")).ToList();
            //我的父节点只有我这一个子节点，而我还要被删除，更新父节点为叶子节点。
            if (myFatherChild.Count == 1)
            {
                sys_api parent = new sys_api
                {
                    id = sys_Api.api_pid,
                    is_leaf = true // //更新父节点为叶子节点。
                };
                _baseService.Update(parent);
            }
            // 删除节点
            _baseService.Remove(sys_Api);
            return MsgModel.Success("删除接口配置成功！");
        }

        /// <summary>
        /// 设置某子节点的所有祖辈id
        /// </summary>
        /// <param name="child"></param>
        private void SetApiIdsAndLevel(sys_api child)
        {
            List<sys_api> allApis = _baseService.GetModels(null).ToList();
            foreach (var sysApi in allApis)
            {
                // 从组织列表中找到自己的直接父亲
                if (sysApi.id == child.api_pid)
                {
                    //直接父亲的所有祖辈id + 直接父id = 当前子节点的所有祖辈id
                    //爸爸的所有祖辈 + 爸爸 = 孩子的所有祖辈
                    child.api_pids = sysApi.api_pids + ",[" + child.api_pid + "]";
                    child.level = sysApi.level + 1;
                }
            }
        }

        /// <summary>
        /// 获取某角色勾选的API访问权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<string> GetCheckedKeys(long roleId)
        {
            return _mySystemService.SelectApiCheckedKeys(roleId);
        }

        /// <summary>
        /// 获取在API分类树中展开的项
        /// </summary>
        /// <returns></returns>
        public List<string> GetExpandedKeys()
        {
            return _mySystemService.SelectApiExpandedKeys();
        }

        /// <summary>
        /// 保存为某角色新勾选的API项
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="checkedIds"></param>
        [Transaction]
        public MsgModel SaveCheckedKeys(long roleId, List<long> checkedIds)
        {
            // 保存之前先删除
            var sysRoleApiList = _baseSysRoleApiService.GetModels(a => a.role_id == roleId);
            _baseSysRoleApiService.DeleteRange(sysRoleApiList);
            _mySystemService.InsertRoleApiIds(roleId, checkedIds);
            return MsgModel.Success("保存接口权限成功！");
        }

        /// <summary>
        /// 接口管理：更新接口的禁用状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MsgModel UpdateStatus(long id, bool status)
        {
            sys_api sys_Api = _baseService.GetModels(a => a.id == id).SingleOrDefault();
            sys_Api.status = status;
            bool result = _baseService.Update(sys_Api);

            return MsgModel.Success(result ? "接口禁用状态更新成功！" : "接口禁用状态更新失败！");
        }
    }
}
