using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using RMDesktopUI.Library.Helpers;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        /// <summary>
        /// დასაწერიმაქვს
        /// </summary>
        /// <returns></returns>
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //make this solid/dry
            var saleDetails = new List<SaleDetailDBModel>();
            var products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate() / 100;

            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetProductById(item.ProductId);

                if (productInfo is null)
                {
                    throw new ArgumentNullException($"The Product id of { detail.ProductId } could not be found in the database");
                }
                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                saleDetails.Add(detail);
            }

            var sale = new SaleDbModel
            {
                CashierId = cashierId,
                SubTotal = saleDetails.Sum(x => x.PurchasePrice),
                Tax = saleDetails.Sum(x => x.Tax),
            };

            sale.Total = sale.SubTotal + sale.Tax;

            var sql = new SqlDataAccess();

            sql.SaveData<SaleDbModel>("dbo.spInsertSale", sale, "RMData");

            sale.Id = sql.LoadData<int, dynamic>("dbo.spSelectSale", new { sale.CashierId, sale.SaleDate }, "RMData").FirstOrDefault();

            foreach (var item in saleDetails)
            {
                item.SaleId = sale.Id;
                sql.SaveData<SaleDetailDBModel>("dbo.spInsertSaleDetail", item, "RMData");
            }

        }
    }
}
