﻿using RMDataManager.Library.Internal.DataAccess;
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
            //make this solid/dryC:\Users\computer\OneDrive\Desktop\Projects\RetailManager\RMDataManager.Library\DataAccess\SaleData.cs
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

            using (var sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMData");
                    //sale mode
                    sql.SaveDataInTransaction("dbo.spInsertSale", sale);
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSelectSale", new { sale.CashierId, sale.SaleDate })
                                 .FirstOrDefault();
                    foreach (var item in saleDetails)
                    {
                        item.SaleId = sale.Id;
                        //sale details
                        sql.SaveDataInTransaction("dbo.spInsertSaleDetail", item);
                    }
                    sql.CommitTransaction();
                }
                catch
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }
        }

        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var data = sql.LoadData<SaleReportModel, dynamic>("dbo.spSaleReport", new { }, "RMData");

            return data;
        }
    }
}
