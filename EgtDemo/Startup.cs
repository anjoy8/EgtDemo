using Autofac;
using BCVP.Sample.Common;
using BCVP.Sample.Extensions;
using BCVP.Sample.IServices;
using EgtDemo.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz.Spi;

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

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
            services.AddTransient<Job_Blogs_Quartz>();//Job使用瞬时依赖注入

        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
            builder.RegisterModule(new BCVPAutofacModuleRegister());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITasksQzServices tasksQzServices, ISchedulerCenter schedulerCenter)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseQuartzJobMildd(tasksQzServices, schedulerCenter);

        }
    }
}
