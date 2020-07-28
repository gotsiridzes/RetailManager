using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            var sql = new SqlDataAccess();
            var data = sql.LoadData<InventoryModel, dynamic>("dbo.spInventorySelectAll", new { }, "RMData");
            return data;
        }

        public void SaveInventory(InventoryModel inventory)
        {
            var sql = new SqlDataAccess();

            sql.SaveData("dbo.spInventoryAdd", inventory, "RMData");
        }
    }
}
