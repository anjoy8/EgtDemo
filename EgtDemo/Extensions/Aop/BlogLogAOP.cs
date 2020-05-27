using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgtDemo.Extensions.Aop
{
    /// <summary>
    /// 拦截器BlogLogAOP 继承IInterceptor接口
    /// </summary>
    public class BlogLogAOP : IInterceptor
    {
     


        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            string UserName = "";

            //记录被拦截方法信息的日志信息
            var dataIntercept = "" +
                $"【当前操作用户】：{ UserName} \r\n" +
                $"【当前执行方法】：{ invocation.Method.Name} \r\n" +
                $"【携带的参数有】： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            try
            {
                //在被拦截的方法执行完毕后 继续执行当前方法，注意是被拦截的是异步的
                invocation.Proceed();

               
                {// 同步1

                    dataIntercept += ($"【执行完成结果】：{invocation.ReturnValue}");
                  
                }
            }
            catch (Exception ex)// 同步2
            {
            }

            Console.WriteLine(dataIntercept);
        }

    
    }
}
