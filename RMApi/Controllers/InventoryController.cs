using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IConfiguration configuration;

        public InventoryController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public List<InventoryModel> Get()
        {
            var data = new InventoryData(configuration);
            return data.GetInventory();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public void Post(InventoryModel inventory)
        {
            var data = new InventoryData(configuration);
            data.SaveInventory(inventory);
        }
    }
}