using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgtDemo.Extensions
{
    public static class HttpContextSetup
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserEgt, AspNetUserEgt>();
        }
    }
}
