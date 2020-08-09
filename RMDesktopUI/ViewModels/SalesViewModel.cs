using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Dynamic;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
	public class SalesViewModel : Screen
    {
		private BindingList<ProductDisplayModel> products;
		private int itemQuantity = 1;
		private IProductEndpoint productEndpoint;
		private BindingList<CartItemDisplayModel> cart = new BindingList<CartItemDisplayModel>();
		private ProductDisplayModel selectedProduct;
		private IConfigHelper configHelper;
		private ISaleEndpoint saleEndpoint;
		private IMapper mapper;
		private StatusInfoViewModel status;
		private IWindowManager window;

		public SalesViewModel(IProductEndpoint productEndpoint, 
			ISaleEndpoint saleEndpoint, 
			IConfigHelper configHelper,
			IMapper mapper,
			StatusInfoViewModel status,
			IWindowManager window
			)
		{
			this.productEndpoint = productEndpoint;
			this.configHelper = configHelper;
			this.saleEndpoint = saleEndpoint;
			this.mapper = mapper;
			this.status = status;
			this.window = window;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			try
			{
				await LoadProducts();
			}
			catch (System.Exception ex)
			{
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.Resize = ResizeMode.NoResize;
				settings.Title = "System Error";

				if (ex.Message == "Unauthorized")
				{
					status.UpdateMessage("Unaothorized Access", "You do not have permission to interact with the sales form");
					window.ShowDialog(status, null, settings);
				}
				else
				{
					status.UpdateMessage("Fatal Exception", ex.Message);
					window.ShowDialog(status, null, settings);
				}

				TryClose();
			}
		}

		private async Task LoadProducts()
		{
			var productsList = await productEndpoint.GetAll();
			var products = mapper.Map<List<ProductDisplayModel>>(productsList);
			Products = new BindingList<ProductDisplayModel>(products);
		}

		public BindingList<ProductDisplayModel> Products
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

		public BindingList<CartItemDisplayModel> Cart
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

		public ProductDisplayModel SelectedProduct
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

		private async Task ResetSalesViewModel()
		{
			Cart = new BindingList<CartItemDisplayModel>();

			await LoadProducts();

			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}

		private CartItemDisplayModel selectedCartItem;

		public CartItemDisplayModel SelectedCartItem
		{
			get
			{
				return selectedCartItem;
			}
			set
			{
				selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
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
			}
			else
			{
				var item = new CartItemDisplayModel
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

				if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
				{
					output = true;
				}

				return output;
			}
		}

		public void RemoveFromCart()
		{
			SelectedCartItem.Product.QuantityInStock += 1;
			
			if(SelectedCartItem.QuantityInCart > 1)
			{
				SelectedCartItem.QuantityInCart -= 1;
			}
			else
			{
				Cart.Remove(SelectedCartItem);
			}


			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
			NotifyOfPropertyChange(() => CanAddToCart);
		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				if (Cart.Count > 0)
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

			await ResetSalesViewModel();
		}
	}
}
