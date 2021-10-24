using Common.Lib.DataHelper;
using System;
using System.Collections.Generic;
using System.Text;
using HomeVideo.DTO.BasicDTO;
using HomeVideo.Util;

namespace HomeVideo.DAL.Role
{
    public class RoleDAL
        : SqlOperationBllAdapter<RoleInfo>
        , IRoleDAL
    {
        protected override string ConnStr => ConnectionStrings.TestConnectiong;
    }
}
