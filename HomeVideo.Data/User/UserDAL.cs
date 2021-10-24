using Common.Lib;
using Common.Lib.DataHelper;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;
using Util;

namespace HomeVideo.DAL.User
{
    public class UserDAL
        : SqlOperationBllAdapter<UserInfo>
        , IUserDAL
    {
        protected override string ConnStr => ConnectionStrings.VideoConnectiong;

        bool IUserDAL.ExistLoginName(string loginName, IDbTransaction transaction)
        {
            try
            {
                Condition condition = new Condition();
                condition.WhereEqual(new string[] {
                    nameof(UserInfo.IsDeleted),
                    nameof(UserInfo.LoginName)
                })
                    .SetTransaction(transaction);

                return Exist(new UserInfo { IsDeleted = false, LoginName = loginName }, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"查询是否已存在登录名失败，loginName：{loginName}", ex);
                return false;
            }
        }

        IEnumerable<UserInfo> IUserDAL.Pages(int pageIndex, int pageSize, out long total, UserInfo filterInstance)
        {
            total = 0;
            List<FilterInfo> filters = new List<FilterInfo>();

            try
            {
                filterInstance ??= new UserInfo();
                filterInstance.IsDeleted = false;
                filters.Add(FilterInfo.Equal(nameof(UserInfo.IsDeleted)));
                if (!filterInstance.UserName.IsNullOrEmpty())
                    filters.Add(FilterInfo.Like(nameof(UserInfo.UserName)));

                PageCondition pageCondition = new PageCondition();
                var join = JoinTable.LeftJoin(nameof(RoleInfo))
                    .On(nameof(RoleInfo.Id), nameof(UserInfo.RoleId), nameof(UserInfo));
                pageCondition.SetPage(pageIndex, pageSize)
                    .SelectExtra(nameof(RoleInfo.RoleName),
                        nameof(UserInfo.Role),
                        nameof(RoleInfo))
                    .LeftJoin(join)
                    .Where(filters);

                return Page(filterInstance, pageCondition, out total);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"查询分页数据失败，类名：{nameof(UserInfo)}，数据：{filterInstance.ToJson()}，筛选条件字段：{filters.ToJson()}，页码：{pageIndex}，页数：{pageSize}", ex);
                return null;
            }
        }

        bool IUserDAL.RoleInUse(int roleId, IDbTransaction dbTransaction)
        {
            try
            {
                Condition condition = new Condition();
                condition.WhereEqual(new string[] {
                    nameof(UserInfo.IsDeleted),
                    nameof(UserInfo.RoleId) })
                    .SetTransaction(dbTransaction);

                return Exist(new UserInfo { IsDeleted = false, RoleId = roleId }, condition);
            }
            catch (Exception ex)
            {
                LoggerHelper.GlobalLogger.Error($"查询角色ID是否正在使用失败，roleId：{roleId}", ex);
                return false;
            }
        }
    }
}
