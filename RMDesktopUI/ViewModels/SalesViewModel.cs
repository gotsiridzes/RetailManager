using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> products;
		private int itemQuantity;
		private IProductEndpoint productEndpoint;

		public BindingList<ProductModel> Products
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

		public SalesViewModel(IProductEndpoint productEndpoint)
		{
			this.productEndpoint = productEndpoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			await LoadProducts();
		}

		private async Task LoadProducts()
		{
			var productsList = await productEndpoint.GetAll();
			Products = new BindingList<ProductModel>(productsList);
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
