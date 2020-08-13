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

		private UserModel selectedUser;

		public UserModel SelectedUser
		{
			get
			{
				return selectedUser;
			}
			set
			{
				selectedUser = value;
				SelectedUserName = value.Email;
				UserRoles.Clear();
				UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
				LoadRoles();
				NotifyOfPropertyChange(() => SelectedUser);
			}
		}

		private string selectedUserName;

		public string SelectedUserName
		{
			get
			{
				return selectedUserName;
			}
			set
			{
				selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}

		private BindingList<string> userRoles = new BindingList<string>();

		public BindingList<string> UserRoles
		{
			get
			{
				return userRoles;
			}
			set
			{
				userRoles = value;
				NotifyOfPropertyChange(() => UserRoles);
			}
		}

		private BindingList<string> avaliableRoles = new BindingList<string>();

		public BindingList<string> AvaliableRoles
		{
			get
			{
				return avaliableRoles;
			}
			set
			{
				avaliableRoles = value;
				NotifyOfPropertyChange(() => AvaliableRoles);
			}
		}

		private string selectedUserRole;

		public string SelectedUserRole
		{
			get
			{
				return selectedUserRole;
			}
			set
			{
				selectedUserRole = value;
				NotifyOfPropertyChange(() => SelectedUserRole);
			}
		}

		private string selectedAvaliableRole;

		public string SelectedAvaliableRole
		{
			get
			{
				return selectedAvaliableRole;
			}
			set
			{
				selectedAvaliableRole = value;
				NotifyOfPropertyChange(() => SelectedAvaliableRole);
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

		private async Task LoadRoles()
		{
			var rolesList = await userEndpoint.GetAllRoles();

			foreach (var role in rolesList)
			{
				if (UserRoles.IndexOf(role.Value) < 0)
				{
					AvaliableRoles.Add(role.Value);
				}
			}
		}

		public async void AddSelectedRole()
		{
			await userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvaliableRole);

			UserRoles.Add(SelectedAvaliableRole);
			AvaliableRoles.Remove(SelectedAvaliableRole);
		}

		public async void RemoveSelectedRole()
		{
			await userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

			AvaliableRoles.Add(SelectedUserRole);
			UserRoles.Remove(SelectedUserRole);
		}
	}
}
