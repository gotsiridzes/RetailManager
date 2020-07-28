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
    public class InventoryController : ApiController
    {
        [HttpGet]
        public List<InventoryModel> Get()
        {
            var data = new InventoryData();
            return data.GetInventory();
        }

        [HttpPost]
        public void Post(InventoryModel inventory)
        {
            var data = new InventoryData();
            data.SaveInventory(inventory);
        }
    }
}
