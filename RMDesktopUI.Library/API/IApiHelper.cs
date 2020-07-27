using RMDesktopUI.Library.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMDesktopUI.Library.API
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        void LogOffUser();
        Task<User> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}