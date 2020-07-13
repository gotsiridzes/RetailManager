using RMDesktopUI.Models;
using System.Threading.Tasks;

namespace RMDesktopUI.Helpers
{
    public interface IApiHelper
    {
        Task<User> Authenticate(string username, string password);
    }
}