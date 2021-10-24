using Common.Lib.AutoFac;
using Common.Lib.DataHelper;
using Drapper.Core.SqlStringHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Drapper.Core;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.DAL.User
{
    public interface IUserDAL
        : IDependency,
         ITransaction,
        IBasicDataOperationWithLock<UserInfo>,
        ISingleColumnsOperation<UserInfo>
    {
        IEnumerable<UserInfo> Pages(int pageIndex, int pageSize, out long total, UserInfo filterInstance = null);

        bool RoleInUse(int roleId, IDbTransaction dbTransaction = null);

        bool ExistLoginName(string loginName, IDbTransaction transaction = null);
    }
}
