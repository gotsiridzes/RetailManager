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
    public class ProductData
    {
        private readonly IConfiguration configuration;

        public ProductData(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// პროდუქტების ინფორმაციის წამოღება მონაცემთა ბაზიდან
        /// </summary>
        /// <returns></returns>
        public List<ProductModel> GetProducts()
        {
            var sql = new SqlDataAccess(configuration);

            var data = sql.LoadData<ProductModel, dynamic>("dbo.spProductSelectAll", new { }, "RMData");

            return data;
        }

        public ProductModel GetProductById(int productId)
        {
            var sql = new SqlDataAccess(configuration);

            var data = sql.LoadData<ProductModel, dynamic>(
                "dbo.spProductSelect",
                new { Id = productId },
                "RMData").FirstOrDefault();

            return data;
        }
    }
}
