using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValid = decimal.TryParse(rateText,
                                            out decimal rate);

            if (!isValid)
                throw new ConfigurationErrorsException("The tax rate is not set up properly !");

            return rate;
        }
    }
}
