using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDataManager.Library.Models
{
    public class ProductModel
    {
        /// <summary>
        /// პროდუქტის უნიკალური იდენტიფიკატორი
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// პროდუქტის სახელი
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// პროდუქტის აღწერა
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// გასაყიდი ფასი
        /// </summary>
        public decimal RetailPrice { get; set; }

        /// <summary>
        /// რაოდენობა საწყობში
        /// </summary>
        public int QuantityInStock { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsTaxable { get; set; }
    }
}
