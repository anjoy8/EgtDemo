using BCVP.Sample.IServices;
using EgtDemo.Extensions;
using EgtDemo.IServ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EgtDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDemoServ _demoServ;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;
        private readonly IHubContext<ChatHub> _hubContext;

        public WeatherForecastController(ISysUserInfoServices sysUserInfoServices, ILogger<WeatherForecastController> logger, IDemoServ demoServ, IRoleModulePermissionServices roleModulePermissionServices,
            IHubContext<ChatHub> hubContext
            )
        {
            _sysUserInfoServices = sysUserInfoServices;
            _logger = logger;
            _demoServ = demoServ;
            _roleModulePermissionServices = roleModulePermissionServices;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", "你好","世界！").Wait();

            var demos = _demoServ.GetDemos();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public async Task<Object> Login(string username)
        {

            //简单验证
            if (string.IsNullOrWhiteSpace(username))
            {
                return Ok(new { code = "failed", msg = "用户名不能为空" });
            }


            //登陆授权
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,username),   //储存用户name
                new Claim(ClaimTypes.NameIdentifier,username)  //储存用户id
            };
            var indentity = new ClaimsIdentity(claims, "formlogin");
            var principal = new ClaimsPrincipal(indentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (principal.Identity.IsAuthenticated)
            {
                return Ok(new { code = "success", msg = "登陆成功" });
            }
            else
                return Ok(new { code = "failed", msg = "登陆失败" });
        }

    }
}
