using Autofac;
using System;
using System.IO;
using System.Reflection;
using Autofac.Extras.DynamicProxy;
using EgtDemo.Model;
using System.Collections.Generic;
using EgtDemo.Extensions;

namespace EgtDemo
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();
            builder.RegisterType<LoveU>();
            var cacheType = new List<Type>();

            builder.RegisterType<BlogLogAOP>();
            cacheType.Add(typeof(BlogLogAOP));



            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "EgtDemo.Serv.dll");
            var repositoryDllFile = Path.Combine(basePath, "EgtDemo.Repo.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                throw new Exception(msg);
            }



           

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。



            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            #endregion


            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(LoveU)))
                .EnableClassInterceptors()
                .InterceptedBy(cacheType.ToArray());

        }
    }
}