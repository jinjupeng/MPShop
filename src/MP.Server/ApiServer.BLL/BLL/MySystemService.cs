using ApiServer.BLL.IBLL;
using ApiServer.DAL.IDAL;
using ApiServer.DAL.UnitOfWork;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.MsgModel;
using ApiServer.Model.Model.PageModel;
using ApiServer.Model.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.BLL.BLL
{
    public class MySystemService : IMySystemService
    {
        private readonly IMySystemDal _mySystemDal;
        protected IUnitOfWork unitOfWork;

        public MySystemService(IMySystemDal mySystemDal, IUnitOfWork unitOfWork)
        {
            _mySystemDal = mySystemDal; 
            this.unitOfWork = unitOfWork;
        }

        public List<string> GetCheckedRoleIds(int userId)
        {
            return _mySystemDal.GetCheckedRoleIds(userId).ToList();
        }

        public int InsertRoleApiIds(int roleId, List<int> checkedIds)
        {
            _mySystemDal.InsertRoleApiIds(roleId, checkedIds);
            return unitOfWork.SaveChanges();
        }

        public int InsertRoleMenuIds(int roleId, List<int> checkedIds)
        {
            _mySystemDal.InsertRoleMenuIds(roleId, checkedIds);
            return unitOfWork.SaveChanges();
        }

        public int InsertUserRoleIds(int userId, List<int> checkedIds)
        {
            _mySystemDal.InsertUserRoleIds(userId, checkedIds);
            return unitOfWork.SaveChanges();
        }

        public List<string> SelectApiCheckedKeys(int roleId)
        {
            return _mySystemDal.SelectApiCheckedKeys(roleId).ToList();
        }

        public List<string> SelectApiExpandedKeys()
        {
            return _mySystemDal.SelectApiExpandedKeys().ToList();
        }

        public List<sys_api> SelectApiTree(int rootApiId, string apiNameLike, bool apiStatus)
        {
            return _mySystemDal.SelectApiTree(rootApiId, apiNameLike, apiStatus).ToList();
        }

        public List<sys_menu> SelectMenuByUserName(string userName)
        {
            return _mySystemDal.SelectMenuByUserName(userName).ToList();
        }

        public List<string> SelectMenuCheckedKeys(int roleId)
        {
            return _mySystemDal.SelectMenuCheckedKeys(roleId).ToList();
        }

        public List<string> SelectMenuExpandedKeys()
        {
            return _mySystemDal.SelectMenuExpandedKeys().ToList();
        }

        public List<sys_menu> SelectMenuTree(int rootMenuId, string menuNameLike, bool? menuStatus)
        {
            return _mySystemDal.SelectMenuTree(rootMenuId, menuNameLike, menuStatus).ToList();
        }

        public List<sys_org> SelectOrgTree(int rootOrgId, string orgNameLike, bool? orgStatus)
        {
            return _mySystemDal.SelectOrgTree(rootOrgId, orgNameLike, orgStatus).ToList();
        }

        public MsgModel SelectUser(int pageIndex, int pageSize, int? orgId, string userName, string phone, string email, bool? enabled, DateTime? createStartTime, DateTime? createEndTime)
        {
            var result = _mySystemDal.SelectUser(orgId, userName, phone, email, enabled, createStartTime, createEndTime);
            int items = result.Count();
            PageModel<SysUserOrg> pageModel = new PageModel<SysUserOrg>
            {
                pageNum = pageIndex,
                size = pageSize,
                records = result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                total = items,
                pageSize = items % pageSize > 0 ? items / pageSize + 1 : items / pageSize // 分页
            };
            return MsgModel.Success(pageModel);
        }
    }
}
