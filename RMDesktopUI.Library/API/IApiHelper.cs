using RMDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public interface IApiHelper
    {
        Task<User> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}