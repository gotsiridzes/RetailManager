using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RMDesktopUI.Views;

namespace RMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        LoginViewModel loginVM;

        public ShellViewModel(LoginViewModel loginVM)
        {
            this.loginVM = loginVM;
            ActivateItem(loginVM);
        }
    }
}
