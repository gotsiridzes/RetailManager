using Caliburn.Micro;
using RMDesktopUI.EventModels;
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

        public ShellViewModel(IEventAggregator events, 
            SalesViewModel salesVM,
            SimpleContainer container,
            ILoggedInUserModel user)
        {
            this.events = events;
            this.salesVM = salesVM;
            this.container = container;
            this.user = user;

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

        public void ExitApplication()
        {
            TryClose();
        }

        public void LogOut()
        {
            user.LogOffUser();
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
