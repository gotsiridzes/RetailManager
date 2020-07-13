using Caliburn.Micro;
using RMDesktopUI.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace RMDesktopUI.ViewModels
{
	public class LoginViewModel : Screen
    {
		private string userName;
		private string password;
		private IApiHelper apiHelper;

		public LoginViewModel(IApiHelper apiHelper)
		{
			this.apiHelper = apiHelper;
		}

		public string UserName
		{
			get 
			{
				return userName; 
			}
			set 
			{
				userName = value;
				NotifyOfPropertyChange(() => UserName);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}


		public string Password
		{
			get 
			{
				return password; 
			}
			set 
			{
				password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}

		public bool IsErrorVisible
		{
			get 
			{
				bool output = false;

				if(ErrorMessage?.Length > 0)
					output = true;

				return output;
			}
			set { }
		}

		private string errorMessage;

		public string ErrorMessage
		{
			get 
			{
				return errorMessage; 
			}
			set 
			{
				errorMessage = value;
				NotifyOfPropertyChange(() => IsErrorVisible);
				NotifyOfPropertyChange(() => ErrorMessage);
			}
		}



		public bool CanLogIn//(string userName, string password)
		{
			get
			{
				bool output = false;

				if (UserName?.Length > 0 & Password?.Length > 0)
					output = true;

				return output;
			}
		}
		
		public async Task LogIn()
		{
			try
			{
				ErrorMessage = string.Empty;
				var result = await apiHelper.Authenticate(UserName, Password);
			
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message; 
			}

		}

	}
}
