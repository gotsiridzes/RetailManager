using Caliburn.Micro;
using RMDesktopUI.EventModels;
using RMDesktopUI.Library.API;
using RMDesktopUI.Library.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator events;
        private SimpleContainer container;
        private ILoggedInUserModel user;
        private IApiHelper apiHelper;

        public ShellViewModel(IEventAggregator events,
                              SimpleContainer container,
                              ILoggedInUserModel user,
                              IApiHelper apiHelper)
        {
            this.events = events;
            this.container = container;
            this.user = user;
            this.apiHelper = apiHelper;

            events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
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
        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
        }

        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }

        public async Task LogOut()
        {
            user.ResetUserModel();
            apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
