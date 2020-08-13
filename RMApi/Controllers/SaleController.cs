using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel saleModel)
        {
            if (saleModel is null)
            {
                throw new ArgumentNullException(nameof(saleModel));
            }
            else
            {
                var data = new SaleData();
                //var userId = RequestContext.Principal.Identity.GetUserId(); // for .net framework web api
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                data.SaveSale(saleModel, userId);
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            var data = new SaleData();
            return data.GetSaleReport();
        }
    }
}