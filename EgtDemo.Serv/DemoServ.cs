using EgtDemo.IRepo;
using EgtDemo.IServ;
using EgtDemo.Model;
using System;

namespace EgtDemo.Serv
{
    public class DemoServ : IDemoServ
    {
        private readonly IDemoRepo _demoRepo;

        public DemoServ(IDemoRepo demoRepo)
        {
            _demoRepo = demoRepo;
        }

        public Demo GetDemos()
        {
            return _demoRepo.GetDemos();
        }
    }
}
