using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EgtDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory()) //<--NOTE THIS
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                     .UseStartup<Startup>()
                     .ConfigureLogging((hostingContext, builder) =>
                     {
                         //过滤掉系统默认的一些日志
                         builder.AddFilter("System", LogLevel.Error);
                         builder.AddFilter("Microsoft", LogLevel.Error);
                         builder.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Error);

                         //可配置文件
                         var path = Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config");
                         builder.AddLog4Net(path);
                     });


                });
    }
}
