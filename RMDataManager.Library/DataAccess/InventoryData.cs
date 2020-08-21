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
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration configuration;
        private readonly ISqlDataAccess sql;

        public InventoryData(IConfiguration configuration, ISqlDataAccess sql)
        {
            this.configuration = configuration;
            this.sql = sql;
        }

        public List<InventoryModel> GetInventory()
        {
            var data = sql.LoadData<InventoryModel, dynamic>("dbo.spInventorySelectAll", new { }, "RMData");
            return data;
        }

        public void SaveInventory(InventoryModel inventory)
        {
            sql.SaveData("dbo.spInventoryAdd", inventory, "RMData");
        }
    }
}
