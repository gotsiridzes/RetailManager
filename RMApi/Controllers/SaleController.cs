using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData saleData;

        public SaleController(ISaleData saleData)
        {
            this.saleData = saleData;
        }

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
                //var userId = RequestContext.Principal.Identity.GetUserId(); // for .net framework web api
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                saleData.SaveSale(saleModel, userId);
            }
        }

        [HttpGet]
        [Route("GetSalesReport")]
        [Authorize(Roles = "Manager,Admin")]
        public List<SaleReportModel> GetSalesReport()
        {
            return saleData.GetSaleReport();
        }
    }
}