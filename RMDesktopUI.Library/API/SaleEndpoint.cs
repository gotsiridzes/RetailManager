using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public class SaleEndpoint : ISaleEndpoint
    {
        private IApiHelper apiHelper;

        public SaleEndpoint(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (var response = await apiHelper.ApiClient.PostAsJsonAsync("api/Sale", sale))
            {
                if (response.IsSuccessStatusCode)
                {
                    //გასაკეთებელია რაღაცეები
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
