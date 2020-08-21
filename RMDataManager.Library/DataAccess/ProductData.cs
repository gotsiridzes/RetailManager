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
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess sql;

        public ProductData(ISqlDataAccess sql)
        {
            this.sql = sql;
        }

        /// <summary>
        /// პროდუქტების ინფორმაციის წამოღება მონაცემთა ბაზიდან
        /// </summary>
        /// <returns></returns>
        public List<ProductModel> GetProducts()
        {
            var data = sql.LoadData<ProductModel, dynamic>("dbo.spProductSelectAll", new { }, "RMData");

            return data;
        }

        public ProductModel GetProductById(int productId)
        {
            var data = sql.LoadData<ProductModel, dynamic>(
                "dbo.spProductSelect",
                new { Id = productId },
                "RMData").FirstOrDefault();

            return data;
        }
    }
}
