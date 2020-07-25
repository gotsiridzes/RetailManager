using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
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

        private int quantityInStock;

        /// <summary>
        /// რაოდენობა საწყობში
        /// </summary>
        public int QuantityInStock
        {
            get 
            {
                return quantityInStock; 
            }
            set 
            {
                quantityInStock = value;
                CallPropertyChanged(nameof(QuantityInStock));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsTaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CallPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }

    }
}
