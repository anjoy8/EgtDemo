using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgtDemo.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EgtDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
        [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserEgt _userEgt;

        public UserController(IUserEgt userEgt)
        {
            _userEgt = userEgt;
        }

        [HttpGet]
        [Authorize]
        public object GetUserInfoWithAuthorize()
        {
            return _userEgt.Name;
        }


        [HttpGet]
        public object GetUserInfoWithoutAuthorize()
        {
            return _userEgt.Name;
        }
    }
}
