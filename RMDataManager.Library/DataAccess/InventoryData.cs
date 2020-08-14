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
    public class InventoryData
    {
        private readonly IConfiguration configuration;

        public InventoryData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<InventoryModel> GetInventory()
        {
            var sql = new SqlDataAccess(configuration);
            var data = sql.LoadData<InventoryModel, dynamic>("dbo.spInventorySelectAll", new { }, "RMData");
            return data;
        }

        public void SaveInventory(InventoryModel inventory)
        {
            var sql = new SqlDataAccess(configuration);

            sql.SaveData("dbo.spInventoryAdd", inventory, "RMData");
        }
    }
}
