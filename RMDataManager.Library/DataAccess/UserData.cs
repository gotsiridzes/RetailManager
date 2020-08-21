using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess sql;

        public UserData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        public List<UserModel> GetUserById(string Id)
        {
            return sql.LoadData<UserModel, dynamic>("dbo.spUserSelect", new { Id }, "RMData");
        }
    }
}
