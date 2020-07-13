using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace RMDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // GET: User/Details/5
        public List<UserModel> GetById()
        {
            var userId = RequestContext.Principal.Identity.GetUserId();

            var data = new UserData();

            return data.GetUserById(userId);
        }
    }
}
