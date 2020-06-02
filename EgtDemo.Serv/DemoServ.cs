using AutoMapper;
using EgtDemo.IRepo;
using EgtDemo.IServ;
using EgtDemo.Model;
using EgtDemo.Model.ViewModels;
using System;

namespace EgtDemo.Serv
{
    public class DemoServ : IDemoServ
    {
        private readonly IDemoRepo _demoRepo;
        private readonly IMapper _mapper;

        public DemoServ(IDemoRepo demoRepo, IMapper mapper)
        {
            _demoRepo = demoRepo;
            _mapper = mapper;
        }

        public DemoViewModel GetDemos()
        {
            return _mapper.Map<DemoViewModel>(_demoRepo.GetDemos());
        }
    }
}
