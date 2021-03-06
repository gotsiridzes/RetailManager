﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMApi.Data;
using RMApi.Models;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserData userData;
        private readonly ILogger<UserController> logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserData userData, ILogger<UserController> logger)
        {
            this.context = context;
            this.userData = userData;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public UserModel GetById()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userData.GetUserById(userId).First();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            var output = new List<ApplicationUserModel>();

            var users = context.Users.ToList();
            var userRoles = from ur in context.UserRoles
                            join r in context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };
            //var roles = context.Roles.ToList();

            foreach (var user in users)
            {
                var u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, value => value.Name);

                output.Add(u);
            }
            return output;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(pairing.UserId);
            logger.LogInformation("Admin {Admin} added user {User} to role {Role}",
                                  loggedInUserId,
                                  user.Id,
                                  pairing.RoleName);
            await userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(pairing.UserId);
            logger.LogInformation("Admin {Admin} removed user {User} to role {Role}",
                      loggedInUserId,
                      user.Id,
                      pairing.RoleName);

            await userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}