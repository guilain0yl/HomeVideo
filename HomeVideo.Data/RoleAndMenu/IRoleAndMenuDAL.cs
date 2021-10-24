using Common.Lib.AutoFac;
using Common.Lib.DataHelper;
using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.DAL.RoleAndMenu
{
    public interface IRoleAndMenuDAL
        : IDependency,
        IQueryOperation<RoleAndMenuInfo>
    {
        bool InsertRoleAndMenuInfos(IEnumerable<RoleAndMenuInfo> roleAndMenuInfos);
    }
}
