using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
		private IConfigHelper configHelper;
		private ISaleEndpoint saleEndpoint;
		
		public SalesViewModel(IProductEndpoint productEndpoint, 
			ISaleEndpoint saleEndpoint, 
			IConfigHelper configHelper)
		{
			this.productEndpoint = productEndpoint;
			this.configHelper = configHelper;
			this.saleEndpoint = saleEndpoint;
		}

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
				decimal subTotal = CalculateSubTotal();

				return subTotal.ToString("C");
			}
		}

		private decimal CalculateSubTotal()
		{
			decimal subTotal = decimal.Zero;

			foreach (var item in Cart)
				subTotal += (item.Product.RetailPrice * item.QuantityInCart);

			return subTotal;
		}

		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();
				return total.ToString("C");
			}
		}

		public string Tax
		{
			get
			{
				decimal taxAmount = CalculateTax();

				return taxAmount.ToString("C");
			}
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = decimal.Zero;
			decimal taxRate = configHelper.GetTaxRate() / 100;

			taxAmount = cart
				.Where(x => x.Product.IsTaxable)
				.Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

			//foreach (var item in Cart)
			//	if (item.Product.IsTaxable)
			//		taxAmount += (item.Product.RetailPrice * item.QuantityInCart * (taxRate));

			return taxAmount;
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
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
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
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				if (Cart.Count >= 0)
					output = true;
				
				return output;
			}
		}

		public async Task CheckOut()
		{
			var sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}

			await saleEndpoint.PostSale(sale);
		}
	}
}
