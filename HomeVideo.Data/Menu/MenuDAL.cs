using Common.Lib;
using Common.Lib.DataHelper;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;
using Util;

namespace HomeVideo.DAL.Menu
{
    public class MenuDAL
        : SqlOperationBllAdapter<MenuInfo>
        , IMenuDAL
    {
        protected override string ConnStr => ConnectionStrings.TestConnectiong;

        IEnumerable<MenuInfo> IMenuDAL.QueryMenusWithIds(IEnumerable<int> menuIds)
        {
            try
            {
                Condition condition = new Condition();
                condition.WhereIn(nameof(MenuInfo.Id), null, "ids");

                return Query(new { ids = menuIds }, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"根据IDS查询菜单失败，menuIds：{menuIds.ToJson()}", ex);
                return null;
            }
        }
    }
}
