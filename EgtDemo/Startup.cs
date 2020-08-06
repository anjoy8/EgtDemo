using Autofac;
using BCVP.Sample.Common;
using BCVP.Sample.Extensions;
using EgtDemo.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EgtDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new Appsettings(Configuration));

            services.AddControllers();

            services.AddBCVPServiceInit(Configuration, Env);
            services.AddBCVPSqlsugarExtensions();

            //Microsoft.AspNetCore.SignalR.Protocols.NewtonsoftJson

            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.AddAuthentication("Cookies")
              .AddCookie(options =>
              {
                  options.LoginPath = "/Account/Login";
                  options.Cookie.Name = "AspnetcoreSessionId";
                  options.Cookie.Path = "/";
                  options.Cookie.HttpOnly = true;
              });
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule(new BCVPAutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/api2/chatHub");

            });
        }
    }
}
