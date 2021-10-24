using BasicController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.Web.Controllers
{
    public class RoleController : BaseController
    {
        public JsonResult AddRole(RoleInfo info)
            => NoDataCaller(roleBLL.AddInfo, info);

        public JsonResult ModifyRole(RoleInfo info)
            => NoDataCaller(roleBLL.UpdateInfo, info);

        public JsonResult DeleteRole(RoleInfo info)
            => NoDataCaller(roleBLL.DeleteInfo, info);

        public JsonResult PageRole(PageArgs<RoleInfo> info)
        {
            var data = roleBLL.Page(info.PageIndex, info.PageSize, out var total, info.Data, Token);

            return data == null ? resultInfo.Fail() : resultInfo.Success(data: PageData.Create(data, total));
        }

        public JsonResult QueryRole()
            => resultInfo.Judge(data: roleBLL.QueryRole(Token));

        public JsonResult QueryPowerByRoleId(int roleId)
            => resultInfo.Judge(data: roleAndMenuBLL.QueryMenuAndRole(roleId));

        public JsonResult ModifyPower(int roleId, IEnumerable<int> menuIds)
            => NoDataCaller(
                roleAndMenuBLL.AddMenuAndRole, menuIds?.Select(x =>
            new RoleAndMenuInfo { RoleId = roleId, MenuId = x })
                );
    }
}
