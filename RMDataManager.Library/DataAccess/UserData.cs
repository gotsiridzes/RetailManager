using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            var sql = new SqlDataAccess();

            var parameters = new { Id = id };

            var data = sql.LoadData<UserModel, dynamic>("dbo.spSelectUser", parameters, "RMData");

            return data;
        }
    }
}
