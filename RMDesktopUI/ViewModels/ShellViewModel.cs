using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator events;
        private SalesViewModel salesVM;
        private SimpleContainer container;
        private ILoggedInUserModel user;
        private IApiHelper apiHelper;

        public ShellViewModel(IEventAggregator events, 
            SalesViewModel salesVM,
            SimpleContainer container,
            ILoggedInUserModel user,
            IApiHelper apiHelper)
        {
            this.events = events;
            this.salesVM = salesVM;
            this.container = container;
            this.user = user;
            this.apiHelper = apiHelper;
            events.Subscribe(this);

            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn 
        { 
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
            set { } 
        }
        public void UserManagement()
        {
            ActivateItem(IoC.Get<UserDisplayViewModel>());
        }

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            user.ResetUserModel();
            apiHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(salesVM);

            NotifyOfPropertyChange(() => IsLoggedIn);
        }

    }
}
