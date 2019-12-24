using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testIg.Models;

namespace testIg.Controllers
{
    public class ProfileViewModel
    {
        public IEnumerable<Claim> Claims { get; set; }
        public string Name { get; set; }
    }
    
    [Route("profile")]
    public class ProfileController : Controller
    {
        [Route("")]
        [Authorize]
        public IActionResult Index()
        {
            var vm = new ProfileViewModel
            {
                Claims = User.Claims,
                Name = User.Identity.Name
            };
            return View(vm);
        }
    }

    
}