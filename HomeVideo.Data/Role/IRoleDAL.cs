using Common.Lib.AutoFac;
using Common.Lib.DataHelper;
using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.DAL.Role
{
    public interface IRoleDAL
        : IDependency, ITransaction,
        ILogicDeleteDataOperation<RoleInfo>,
        IBasicDataOperation<RoleInfo>,
        IQueryOperation<RoleInfo>
    {
    }
}
