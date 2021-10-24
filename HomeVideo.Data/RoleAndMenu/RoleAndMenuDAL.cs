using Common.Lib;
using Common.Lib.DataHelper;
using Drapper.Core.DBOperation;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;
using Util;

namespace HomeVideo.DAL.RoleAndMenu
{
    public class RoleAndMenuDAL
        : SqlOperationAdapter<RoleAndMenuInfo>,
        IRoleAndMenuDAL
    {
        protected override string ConnStr => ConnectionStrings.TestConnectiong;

        bool IRoleAndMenuDAL.InsertRoleAndMenuInfos(IEnumerable<RoleAndMenuInfo> roleAndMenuInfos)
        {
            if (roleAndMenuInfos == null || roleAndMenuInfos.Count() <= 0)
            {
                throw new ArgumentNullException("插入权限异常");
            }

            var tran = BeginTransaction();

            try
            {
                Condition condition = new Condition();
                condition.WhereEqual(nameof(RoleAndMenuInfo.RoleId))
                    .SetTransaction(tran);

                Delete(new RoleAndMenuInfo { RoleId = roleAndMenuInfos.First().RoleId }, condition);

                if (BatchInsert(roleAndMenuInfos, tran) < 1)
                {
                    RollbackTransaction(tran);
                    return false;
                }

                CommitTransaction(tran);
                return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction(tran);
                LoggerHelper.GlobalLogger.Error($"更新权限失败，roleAndMenuInfos：{roleAndMenuInfos.ToJson()}", ex);
                return false;
            }
        }
    }
}
