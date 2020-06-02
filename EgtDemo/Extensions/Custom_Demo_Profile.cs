using AutoMapper;
using EgtDemo.Model;
using EgtDemo.Model.ViewModels;

namespace EgtDemo.Extensions
{
    public class Custom_Demo_Profile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public Custom_Demo_Profile()
        {
            CreateMap<Demo, DemoViewModel>().ForMember(d => d.DemoName, o => o.MapFrom(s => s.name));
            CreateMap<DemoViewModel, Demo>().ForMember(d => d.name, o => o.MapFrom(s => s.DemoName));
        }
    }
}
