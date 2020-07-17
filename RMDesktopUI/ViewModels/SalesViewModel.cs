using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<string> products;
		private int itemQuantity;

		public BindingList<string> Products
		{
			get 
			{
				return products; 
			}
			set 
			{
				products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}


		public int ItemQuantity
		{
			get 
			{
				return itemQuantity; 
			}
			set 
			{
				itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
			}
		}

		private string cart;

		public string Cart
		{
			get 
			{
				return cart; 
			}
			set 
			{
				cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public decimal SubTotal
		{
			get 
			{
				// შესაცვლელია გამოთვლებით
				return 0;			
			}
		}

		public decimal Total
		{
			get
			{
				// შესაცვლელია გამოთვლებით
				return 0;
			}
		}

		public decimal Tax
		{
			get
			{
				// შესაცვლელია გამოთვლებით
				return 0;
			}
		}


		public bool CanAddToCart 
		{
			get 
			{
				bool output = false;

				// უნდა შემოწმდეს რომ არჩეულია ნივთი
				// უნდა შემოწმდეს რომ რაოდენობა ცარიელი არ არის

				return output;
			} 
		}

		public void AddToCart()
		{

		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				// უნდა შემოწმდეს რომ არჩეულია ნივთი
				// უნდა შემოწმდეს რომ რაოდენობა ცარიელი არ არის

				return output;
			}
		}

		public void RemoveFromCart()
		{

		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				// უნდა შემოწმდეს რომ არჩეულია ნივთი
				// უნდა შემოწმდეს რომ რაოდენობა ცარიელი არ არის

				return output;
			}
		}

		public void CheckOut()
		{

		}



	}
}
