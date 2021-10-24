using Common.Lib.AutoFac;
using Common.Lib.DataHelper;
using Drapper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;

namespace HomeVideo.DAL.Menu
{
    public interface IMenuDAL
        : IDependency, ITransaction,

        IBasicDataOperationWithLock<MenuInfo>,
        ISingleColumnsOperation<MenuInfo>,
        IQueryOperation<MenuInfo>
    {
        IEnumerable<MenuInfo> QueryMenusWithIds(IEnumerable<int> menuIds);
    }
}
