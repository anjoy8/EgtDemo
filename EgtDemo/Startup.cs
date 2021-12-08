using Autofac;
using BCVP.Sample.Common;
using BCVP.Sample.Extensions;
using EgtDemo.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

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

            //services.AddAuthentication("Cookies")
            //  .AddCookie(options =>
            //  {
            //      options.LoginPath = "/Account/Login";
            //      options.Cookie.Name = "AspnetcoreSessionId";
            //      options.Cookie.Path = "/";
            //      options.Cookie.HttpOnly = true;
            //  });


            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // However, we want JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidIssuer = "Issuer",
                   ValidAudience = "Audience",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("asdfghjkl;1234567890"))
               };

               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Query["access_token"];

                       // If the request is for our hub...
                       var path = context.HttpContext.Request.Path;
                       if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/api2/chatHub")))
                       {
                           // Read the token out of the query string
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   }
               };
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
