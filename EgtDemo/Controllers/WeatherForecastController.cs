using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCVP.Sample;
using BCVP.Sample.Common.Helper;
using BCVP.Sample.IServices;
using BCVP.Sample.Model;
using BCVP.Sample.Model.ViewModels;
using EgtDemo.AuthHelper;
using EgtDemo.IServ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly PermissionRequirement _requirement;
        private readonly IDemoServ _demoServ;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;

        public WeatherForecastController(ISysUserInfoServices sysUserInfoServices, ILogger<WeatherForecastController> logger, PermissionRequirement requirement, IDemoServ demoServ, IRoleModulePermissionServices roleModulePermissionServices)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _logger = logger;
            _requirement = requirement;
            _demoServ = demoServ;
            _roleModulePermissionServices = roleModulePermissionServices;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
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
        public async Task<MessageModel<TokenInfoViewModel>> Token(string name = "", string pass = "")
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pass))
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "用户名或密码不能为空",
                };
            }

            pass = MD5Helper.MD5Encrypt32(pass);

            var user = await _sysUserInfoServices.Query(d => d.uLoginName == name && d.uLoginPWD == pass && d.tdIsDelete == false);
            if (user.Count > 0)
            {
                var userRoles = await _sysUserInfoServices.GetUserRoleNameStr(name, pass);
                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Jti, user.FirstOrDefault().uID.ToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(userRoles.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));


                // ids4和jwt切换
                // jwt
                if (!Permissions.IsUseIds4)
                {
                    var data = await _roleModulePermissionServices.RoleModuleMaps();
                    var list = (from item in data
                                where item.IsDeleted == false
                                orderby item.Id
                                select new PermissionItem
                                {
                                    Url = item.Module?.LinkUrl,
                                    Role = item.Role?.Name.ObjToString(),
                                }).ToList();

                    _requirement.Permissions = list;
                }

                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = true,
                    msg = "获取成功",
                    response = token
                };
            }
            else
            {
                return new MessageModel<TokenInfoViewModel>()
                {
                    success = false,
                    msg = "认证失败",
                };
            }
        }
    }
}
