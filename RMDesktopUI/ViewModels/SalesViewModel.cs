using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> products;
		private int itemQuantity = 1;
		private IProductEndpoint productEndpoint;
		private BindingList<CartItemModel> cart = new BindingList<CartItemModel>();
		private ProductModel selectedProduct;

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
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}


		public BindingList<CartItemModel> Cart
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

		public string SubTotal
		{
			get 
			{
				decimal subTotal = 0;

				foreach (var item in Cart)
					subTotal += (item.Product.RetailPrice * item.QuantityInCart);
				return subTotal.ToString("C");			
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


		public ProductModel SelectedProduct
		{
			get 
			{
				return selectedProduct; 
			}
			set 
			{
				selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
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

				if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}

				return output;
			} 
		}

		public void AddToCart()
		{
			var existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
				Cart.Remove(existingItem);
				Cart.Add(existingItem);
			}
			else
			{
				var item = new CartItemModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};

				Cart.Add(item);
			}

			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
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
			 
			NotifyOfPropertyChange(() => SubTotal);
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
