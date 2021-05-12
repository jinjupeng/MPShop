using ApiServer.BLL.IBLL;
using ApiServer.Common;
using ApiServer.Common.Attributes;
using ApiServer.Model.Entity;
using ApiServer.Model.Enum;
using ApiServer.Model.Model.DataTree;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.Nodes;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.BLL.BLL
{
    public class SysOrgService : ISysOrgService
    {
        private readonly IBaseService<sys_org> _baseSysOrgService;
        private readonly IMySystemService _mySystemService;

        public SysOrgService(IBaseService<sys_org> baseSysOrgService, IMySystemService mySystemService)
        {
            _baseSysOrgService = baseSysOrgService;
            _mySystemService = mySystemService;
        }

        /// <summary>
        /// 根据当前登录用户所属组织，查询组织树
        /// </summary>
        /// <param name="rootOrgId">当前登录用户的组织id</param>
        /// <param name="orgNameLike">组织名称参数</param>
        /// <param name="orgStatus">组织状态参数</param>
        /// <returns>组织列表</returns>
        public MsgModel GetOrgTreeById(long rootOrgId, string orgNameLike, bool? orgStatus)
        {
            MsgModel msg = new MsgModel()
            {
                isok = true,
                message = "查询成功！"
            };
            List<sys_org> sysOrgs = _mySystemService.SelectOrgTree(rootOrgId, orgNameLike, orgStatus);
            List<SysOrgNode> sysOrgNodes = new List<SysOrgNode>();
            foreach (sys_org sys_org in sysOrgs)
            {
                SysOrgNode sysOrgNode = sys_org.BuildAdapter().AdaptToType<SysOrgNode>();
                sysOrgNodes.Add(sysOrgNode);
            }
            if (!string.IsNullOrEmpty(orgNameLike))
            {
                //根据组织名称查询，返回平面列表
                msg.data = sysOrgNodes;
                return msg;
            }
            else
            {
                //否则返回树型结构列表
                msg.data = DataTreeUtil<SysOrgNode, long>.BuildTree(sysOrgNodes, rootOrgId);
                return msg;
            }

        }

        public MsgModel UpdateOrg(sys_org sys_org)
        {
            if (!_baseSysOrgService.Update(sys_org))
            {
                return MsgModel.Fail("更新组织机构失败！");
            }

            return MsgModel.Success("更新组织机构成功！");
        }

        [Transaction]
        public MsgModel AddOrg(sys_org sys_org)
        {
            sys_org.id = new Snowflake().GetId();
            SetOrgIdsAndLevel(sys_org);
            sys_org.is_leaf = true;//新增的组织节点都是子节点，没有下级
            sys_org parent = _baseSysOrgService.GetModels(a => a.id == sys_org.org_pid).SingleOrDefault();
            parent.id = sys_org.org_pid;
            parent.is_leaf = false; //更新父节点为非子节点。
            _baseSysOrgService.Update(parent);

            sys_org.status = false;//设置是否禁用，新增节点默认可用
            _baseSysOrgService.Add(sys_org);
            return MsgModel.Success("新增组织机构成功！");
        }

        [Transaction]
        public MsgModel DeleteOrg(sys_org sys_org)
        {
            List<sys_org> myChilds = _baseSysOrgService.GetModels(a => a.org_pids.Contains("[" + sys_org.org_pid + "]")).ToList();
            if (myChilds.Count > 0)
            {
                // "不能删除有下级组织的组织机构"
                throw new CustomException((int)HttpStatusCode.Status500InternalServerError, "不能删除有下级组织的组织机构");
            }
            List<sys_org> myFatherChilds = _baseSysOrgService.GetModels(a => a.org_pids.Contains("[" + "]")).ToList();
            //我的父节点只有我这一个子节点，而我还要被删除，更新父节点为叶子节点。
            if (myFatherChilds.Count == 1)
            {
                sys_org parent = new sys_org
                {
                    id = sys_org.org_pid,
                    is_leaf = true// 更新父节点为叶子节点。
                };
                _baseSysOrgService.Update(parent);
            }
            // 删除节点
            _baseSysOrgService.Remove(sys_org);
            return MsgModel.Success("删除组织机构成功！");
        }

        /// <summary>
        /// //设置某子节点的所有祖辈id
        /// </summary>
        /// <param name="child"></param>
        private void SetOrgIdsAndLevel(sys_org child)
        {
            List<sys_org> allOrgs = _baseSysOrgService.GetModels(null).ToList();
            foreach (sys_org sys_org in allOrgs)
            {
                //从组织列表中找到自己的直接父亲
                if (sys_org.id == child.org_pid)
                {
                    //直接父亲的所有祖辈id + 直接父id = 当前子节点的所有祖辈id
                    //爸爸的所有祖辈 + 爸爸 = 孩子的所有祖辈
                    child.org_pids = sys_org.org_pids + "[" + child.org_pid + "]";
                    child.level = sys_org.level + 1;
                }
            }
        }

        /// <summary>
        /// 组织管理：更新组织的禁用状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        public MsgModel UpdateStatus(long id, bool status)
        {
            sys_org sys_org = _baseSysOrgService.GetModels(a => a.id == id).SingleOrDefault();
            sys_org.status = status;
            bool result = _baseSysOrgService.Update(sys_org);

            return result ? MsgModel.Success("更新组织机构状态成功！") : MsgModel.Fail("更新组织机构状态失败！");
        }
    }
}
