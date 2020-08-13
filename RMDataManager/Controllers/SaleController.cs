using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [HttpPost]
        [Authorize(Roles ="Cashier")]
        public void Post(SaleModel saleModel)
        {
            if (saleModel is null)
            {
                throw new ArgumentNullException(nameof(saleModel));
            }
            else
            {
                var data = new SaleData();
                var userId = RequestContext.Principal.Identity.GetUserId();
                data.SaveSale(saleModel, userId);
            }
        }

        [Authorize(Roles ="Manager,Admin")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            var data = new SaleData();
            return data.GetSaleReport();
        } 
    }
}
