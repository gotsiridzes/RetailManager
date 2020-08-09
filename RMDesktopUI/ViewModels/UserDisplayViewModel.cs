using Caliburn.Micro;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
		private StatusInfoViewModel status;
		private IWindowManager window;
		private IUserEndpoint userEndpoint;
		private BindingList<UserModel> users;

		public BindingList<UserModel> Users
		{
			get
			{
				return users;
			}
			set
			{
				users = value;
				NotifyOfPropertyChange(() => Users);
			}

		}

		public UserDisplayViewModel(StatusInfoViewModel status, 
			IWindowManager window, 
			IUserEndpoint userEndpoint)
		{
			this.status = status;
			this.window = window;
			this.userEndpoint = userEndpoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);

			try
			{
				await LoadUsers();
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

		private async Task LoadUsers()
		{
			var userList = await userEndpoint.GetAll();
			Users = new BindingList<UserModel>(userList);
		}
	}
}
