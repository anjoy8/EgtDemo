using BCVP.Sample.IServices;
using EgtDemo.Extensions;
using EgtDemo.IServ;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        public IEnumerable<WeatherForecast> Get(string access_token = "")
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", "你好", "世界！").Wait();

            var user = GetUserInfoFromToken(ClaimTypes.NameIdentifier, access_token).FirstOrDefault();

            _hubContext.Clients.User(user).SendAsync("ReceiveMessage", user, "发奖啦");


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

        public List<string> GetUserInfoFromToken(string ClaimType, string access_token)
        {

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(access_token))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(access_token);

                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }
            else
            {
                return new List<string>() { };
            }
        }

        [HttpGet]
        public string GetToken(string username)
        {
            string iss = "Issuer";
            string aud = "Audience";
            string secret = "asdfghjkl;1234567890";

            //var claims = new Claim[] //old
            var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name,username),   //储存用户name
                new Claim(ClaimTypes.NameIdentifier,username),  //储存用户name
                new Claim(JwtRegisteredClaimNames.Jti, "1"),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
              
                //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(1000).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),

               };



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
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
