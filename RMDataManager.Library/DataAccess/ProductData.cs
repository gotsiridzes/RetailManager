using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData
    {
        /// <summary>
        /// პროდუქტების ინფორმაციის წამოღება მონაცემთა ბაზიდან
        /// </summary>
        /// <returns></returns>
        public List<ProductModel> GetProducts()
        {
            var sql = new SqlDataAccess();

            var data = sql.LoadData<ProductModel, dynamic>("dbo.spSelectProducts", new { }, "RMData");

            return data;
        }
    }
}
