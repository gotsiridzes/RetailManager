using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryData inventoryData;

        public InventoryController(IInventoryData inventoryData)
        {
            this.inventoryData = inventoryData;
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public List<InventoryModel> Get()
        {
            return inventoryData.GetInventory();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel inventory)
        {
            inventoryData.SaveInventory(inventory);
        }
    }
}