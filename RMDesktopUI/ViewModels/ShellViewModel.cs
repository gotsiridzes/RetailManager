using Caliburn.Micro;
using RMDesktopUI.EventModels;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator events;
        private SalesViewModel salesVM;
        private SimpleContainer container;

        public ShellViewModel(IEventAggregator events, 
            SalesViewModel salesVM,
            SimpleContainer container)
        {
            this.events = events;
            this.salesVM = salesVM;
            this.container = container;

            events.Subscribe(this);

            ActivateItem(container.GetInstance<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            ActivateItem(salesVM);
        }
    }
}
