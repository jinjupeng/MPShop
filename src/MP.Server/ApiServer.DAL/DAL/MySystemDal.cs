using ApiServer.Common;
using ApiServer.DAL.IDAL;
using ApiServer.Model.Entity;
using ApiServer.Model.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiServer.DAL.DAL
{
    public class MySystemDal : IMySystemDal
    {
        /// <summary>
        /// EF上下文对象
        /// </summary>
        private readonly ContextMySql _context;

        public MySystemDal(ContextMySql context)
        {
            this._context = context;
        }


        public IQueryable<string> GetCheckedRoleIds(long userId)
        {
            // FormattableString sql = $"SELECT distinct role_id FROM sys_user_role ra WHERE ra.user_id = {userId};";
            // var userRoles = DbContext.Set<sys_user_role>().FromSqlInterpolated(sql).AsNoTracking().AsQueryable();
            var userRoles = _context.Set<sys_user_role>().Where(a => a.user_id == userId);
            var list = new List<string>();
            foreach (var userRole in userRoles)
            {
                list.Add(Convert.ToString(userRole.role_id));
            }
            return list.AsQueryable();

        }

        public int InsertRoleApiIds(int roleId, List<int> checkedIds)
        {
            var sysRoleApis = new List<sys_role_api>();
            foreach(var item in checkedIds)
            {
                var sysRoleApi = new sys_role_api
                {
                    role_id = roleId,
                    api_id = item
                };
                sysRoleApis.Add(sysRoleApi);
            }
            _context.Set<sys_role_api>().AddRange(sysRoleApis);
            
            return checkedIds.Count;

            //string sql = string.Empty;
            //foreach (var checkedId in checkedIds)
            //{
            //    sql += $"INSERT INTO sys_role_api (role_id, api_id) VALUES({roleId}, {checkedId})";
            //}
            //return _context.Database.ExecuteSqlRaw(sql);
        }

        public int InsertRoleMenuIds(int roleId, List<int> checkedIds)
        {
            var sysRoleMenus = new List<sys_role_menu>();
            foreach (var item in checkedIds)
            {
                var sysRoleMenu = new sys_role_menu
                {
                    role_id = roleId,
                    menu_id = item
                };
                sysRoleMenus.Add(sysRoleMenu);
            }
            _context.Set<sys_role_menu>().AddRange(sysRoleMenus);

            return checkedIds.Count;

            //string sql = string.Empty;
            //foreach (var checkedId in checkedIds)
            //{
            //    sql += $"INSERT INTO sys_role_menu (role_id, menu_id) VALUES({roleId}, {checkedId});";
            //}
            //return _context.Database.ExecuteSqlRaw(sql);
        }

        public int InsertUserRoleIds(int userId, List<int> checkedIds)
        {
            var sysUserRoles = new List<sys_user_role>();
            foreach (var item in checkedIds)
            {
                var sysUserRole = new sys_user_role
                {
                    user_id = userId,
                    role_id = item
                };
                sysUserRoles.Add(sysUserRole);
            }
            _context.Set<sys_user_role>().AddRange(sysUserRoles);

            return checkedIds.Count;

            //string sql = string.Empty;
            //foreach (var checkedId in checkedIds)
            //{
            //    sql += $"INSERT INTO sys_user_role (role_id, user_id) VALUES({checkedId}, {userId});";
            //}
            //return _context.Database.ExecuteSqlRaw(sql);
        }

        public IQueryable<string> SelectApiCheckedKeys(long roleId)
        {
            // FormattableString sql = $"SELECT distinct api_id FROM sys_role_api ra WHERE role_id = {roleId}";
            // var roleApis = DbContext.Set<sys_role_api>().FromSqlInterpolated(sql).AsNoTracking().AsQueryable();
            var roleApis = _context.Set<sys_role_api>().Where(a => a.role_id == roleId);
            var list = new List<string>();
            foreach (var sysRoleApi in roleApis)
            {
                list.Add(Convert.ToString(sysRoleApi.api_id));
            }
            return list.AsQueryable();
        }

        public IQueryable<string> SelectApiExpandedKeys()
        {
            // string sql = "SELECT distinct id FROM sys_api a WHERE a.level = 2";
            // var sysApis = DbContext.Set<sys_api>().FromSqlRaw(sql).AsNoTracking().AsQueryable();
            var sysApis = _context.Set<sys_api>().Where(a => a.level == 2);
            var list = new List<string>();
            foreach (var sysApi in sysApis)
            {
                list.Add(Convert.ToString(sysApi.id));
            }
            return list.AsQueryable();
        }

        public IQueryable<sys_api> SelectApiTree(long rootApiId, string apiNameLike, bool apiStatus)
        {
            string sql = $"SELECT id,api_pid,api_pids,is_leaf,api_name,url,sort,level,status FROM sys_api o " +
            $"WHERE (api_pids like CONCAT('%[{rootApiId}]%') OR id = {rootApiId}) ";
            if (apiNameLike != null && apiNameLike != "")
            {
                sql += $"AND api_name like CONCAT('%{apiNameLike}%') ";
            }

            sql += $"AND status = {(apiStatus ? 1 : 0)} ";
            sql += $"ORDER BY level,sort";

            return _context.Set<sys_api>().FromSqlRaw(sql).AsNoTracking().AsQueryable();

        }

        public IQueryable<sys_menu> SelectMenuByUserName(string userName)
        {
            FormattableString sql = $@"
            SELECT distinct m.id,menu_pid,menu_pids,is_leaf,menu_name,url,icon,sort,level,status
            FROM sys_menu m
            LEFT JOIN sys_role_menu rm ON m.id = rm.menu_id
            LEFT JOIN sys_user_role ur ON ur.role_id = rm.role_id
            LEFT JOIN sys_user u ON u.id = ur.user_id
            WHERE u.username = {userName}
            AND m.status = 0
            ORDER BY level,sort
            ";
            return _context.Set<sys_menu>().FromSqlInterpolated(sql).AsNoTracking().AsQueryable();

        }

        public IQueryable<string> SelectMenuCheckedKeys(long roleId)
        {
            // FormattableString sql = $"SELECT distinct menu_id FROM sys_role_menu ra WHERE ra.role_id = {roleId}";
            // var roleMenus = DbContext.Set<sys_role_menu>().FromSqlInterpolated(sql).AsNoTracking().AsQueryable();
            var roleMenus = _context.Set<sys_role_menu>().Where(a => a.role_id == roleId);
            var list = new List<string>();
            foreach (var roleMenu in roleMenus)
            {
                list.Add(Convert.ToString(roleMenu.menu_id));
            }
            return list.AsQueryable();

        }

        public IQueryable<string> SelectMenuExpandedKeys()
        {
            // FormattableString sql = $"SELECT distinct id FROM sys_menu a WHERE a.level = 2";
            // var sysMenus = DbContext.Set<sys_menu>().FromSqlInterpolated(sql).AsNoTracking().AsQueryable();
            var sysMenus = _context.Set<sys_menu>().Where(a => a.level == 2);
            var list = new List<string>();
            foreach (var sysMenu in sysMenus)
            {
                list.Add(Convert.ToString(sysMenu.id));
            }
            return list.AsQueryable();
        }

        public IQueryable<sys_menu> SelectMenuTree(long rootMenuId, string menuNameLike, bool? menuStatus)
        {
            string sql = $"SELECT id,menu_pid,menu_pids,is_leaf,menu_name,url,icon,sort,level,status FROM sys_menu o " +
            $"WHERE (menu_pids like CONCAT('%[{rootMenuId}]%') OR id = {rootMenuId}) ";
            if (menuNameLike != null && menuNameLike != "")
            {
                sql += $"AND menu_name like CONCAT('%{menuNameLike}%') ";
            }
            if (menuStatus != null)
            {
                sql += $"AND status = {((bool)menuStatus ? 1 : 0)} ";
            }
            sql += $"ORDER BY level,sort";

            return _context.Set<sys_menu>().FromSqlRaw(sql).AsNoTracking().AsQueryable();

        }

        public IQueryable<sys_org> SelectOrgTree(long rootOrgId, string orgNameLike, bool? orgStatus)
        {
            string sql = $"SELECT id,org_pid,org_pids,is_leaf,org_name,address,phone,email,sort,level,status FROM sys_org o " +
            $"WHERE (org_pids like CONCAT('%[{rootOrgId}]%') OR id = {rootOrgId}) ";
            if (orgNameLike != null && orgNameLike != "")
            {
                sql += $"AND org_name like CONCAT('%{orgNameLike}%') ";
            }
            if (orgStatus != null)
            {
                sql += $"AND status = {((bool)orgStatus ? 1 : 0)} ";
            }

            sql += $"ORDER BY level,sort";

            return _context.Set<sys_org>().FromSqlRaw(sql).AsNoTracking().AsQueryable();

        }

        //public IQueryable<string> GetCheckedRoleIds(long userId)
        //{
        //    // FormattableString sql = $"SELECT distinct role_id FROM sys_user_role ra WHERE ra.user_id = {userId};";
        //    // return DbContext.Set<string>().FromSqlInterpolated(sql).AsNoTracking().AsQueryable();
        //    var sysMenus = DbContext.Set<sys_user_role>().Where(a => a.user_id == userId);
        //    var list = new List<string>();
        //    foreach (var sysMenu in sysMenus)
        //    {
        //        list.Add(Convert.ToString(sysMenu.role_id));
        //    }
        //    return list.AsQueryable();
        //}

        public IQueryable<SysUserOrg> SelectUser(long? orgId,
                                      string userName,
                                      string phone,
                                      string email,
                                      bool? enabled,
                                      DateTime? createStartTime,
                                      DateTime? createEndTime)
        {
            // https://www.cnblogs.com/wanghaibin/p/6494309.html

            var userList = _context.Set<sys_user>().AsNoTracking();
            var orgList = _context.Set<sys_org>().AsNoTracking();
            var userOrgList = (from u in userList
                               join o in orgList on u.org_id equals o.id
                               where string.IsNullOrEmpty(userName) || u.username.Contains(userName)
                               where string.IsNullOrEmpty(phone) || u.phone.Contains(phone)
                               where string.IsNullOrEmpty(email) || u.email.Contains(email)
                               where enabled == null || u.enabled == enabled
                               where createStartTime == null || createEndTime == null || u.create_time >= createStartTime && u.create_time <= createEndTime
                               where orgId == null || o.id == orgId || o.org_pids.Contains("[" + orgId + "]")
                               select new SysUserOrg
                               {
                                   id = u.id,
                                   username = u.username,
                                   org_id = u.org_id,
                                   OrgName = o.org_name,
                                   enabled = u.enabled,
                                   phone = u.phone,
                                   email = u.email,
                                   create_time = u.create_time
                               });

            return userOrgList;
            //string sql = @"SELECT u.id,u.username,u.org_id,o.org_name,u.enabled,u.phone,u.email,u.create_time
            //            FROM sys_user u
            //            LEFT JOIN sys_org o ON u.org_id = o.id";

            //var result = DbContext.Set<SysUserOrg>().FromSqlRaw(sql).AsNoTracking().AsQueryable();
            //if (!string.IsNullOrWhiteSpace(userName))
            //{
            //    result = result.Where(a => a.username.Contains(userName));
            //}
            //if (!string.IsNullOrWhiteSpace(phone))
            //{
            //    result = result.Where(a => a.phone.Contains(phone));
            //}
            //if (!string.IsNullOrWhiteSpace(email))
            //{
            //    result = result.Where(a => a.email.Contains(email));
            //}
            //if (enabled != null)
            //{
            //    result = result.Where(a => a.enabled == enabled);
            //}
            //if (createStartTime != null && createEndTime != null)
            //{
            //    result = result.Where(a => a.create_time >= createStartTime && a.create_time <= createEndTime);
            //}
            //if (orgId != null)
            //{
            //    result = result.Where(a => a.id == orgId || a.org_pids.Contains("[" + orgId + "]"));
            //}

        }
    }
}
