﻿using Fast.Framework.Model;
using Fast.Framework.Security;
using Fast.Mapping;
using Fast.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fast.Framework
{
    public class WorkContext
    {
        readonly static string ITEMS_CATEGORYIES_KEY = "ab.items.categories";

        SysUserAuthentication _sysUserAuthentication;
        SysRoleService _sysRoleService;
        SysCategoryService _sysCategoryService;
        IHttpContextAccessor _httpContextAccessor;
        SysUserService _sysUserService;

        public WorkContext(SysUserAuthentication sysUserAuthentication,
            SysRoleService sysRoleService,
            IHttpContextAccessor httpContextAccessor,
            SysCategoryService sysCategoryService,
            SysUserService sysUserService)
        {
            _sysRoleService = sysRoleService;
            _sysCategoryService = sysCategoryService;
            _sysUserAuthentication = sysUserAuthentication;
            _httpContextAccessor = httpContextAccessor;
            _sysUserService = sysUserService;
        }

        /// <summary>
        /// 获取保存在cookie或者jwt的简单用户信息
        /// </summary>
        public UserData GetUserData(int platform)
        {
            return _sysUserAuthentication.GetData(platform);
        }

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="platform"></param>
        public void SignOut(int platform = 0)
        {
            _sysUserAuthentication.SignOut(platform);
        }

        /// <summary>
        /// 获取登陆用户的ID
        /// </summary>
        /// <returns></returns>
        public Guid GetUserId(int platform = 0)
        {
            return _sysUserAuthentication.GetUserId(platform);
        }

        /// <summary>
        /// 获取我的权限
        /// </summary>
        /// <param name="plaftorm"></param>
        /// <returns></returns>
        public List<Sys_CategoryMapping> GetMyCategories(int plaftorm = 0)
        {
            var data = _httpContextAccessor.HttpContext.Items[ITEMS_CATEGORYIES_KEY] as List<Sys_CategoryMapping>;
            if (data == null)
            {
                var user = GetUserData(plaftorm);
                var categories = _sysCategoryService.GetAllCache().Where(o => o.Target == plaftorm.ToString()).ToList();
                if (user.IsAdmin)
                {
                    return categories;
                }
                var userPermissions = _sysRoleService.GetUserPermissions(user.Id);
                if (userPermissions != null)
                {
                    data = userPermissions.Join(categories, up => up.CategoryId, c => c.Id, (a, b) => b).Distinct().ToList();
                    if (data != null)
                        _httpContextAccessor.HttpContext.Items[ITEMS_CATEGORYIES_KEY] = data;
                }
            }
            return data;
        }

        /// <summary>
        /// 获取当前已经登陆的信息
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public Sys_UserMapping GetUser(int platform = 0)
        {
            var userId = GetUserId(platform);
            return _sysUserService.GetLogged(userId);
        }

        /// <summary>
        /// 网页端检测是否含有权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool AuthorityCheck(string routeNameOrPath)
        {
            var categories = GetMyCategories(0);
            return categories != null && categories.Any(o => o.RouteName.Equals(routeNameOrPath, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 权限检测
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public bool AuthorityCheck(string action, string controller)
        {
            var categories = GetMyCategories(0);
            return categories != null && categories.Any(o => o.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase)
            && o.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}
