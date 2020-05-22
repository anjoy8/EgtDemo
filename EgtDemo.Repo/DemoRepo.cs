using EgtDemo.IRepo;
using EgtDemo.Model;
using System;

namespace EgtDemo.Repo
{
    public class DemoRepo : IDemoRepo
    {
        public Demo GetDemos()
        {
            return new Demo()
            {
                id = 1,
                name = "anson"
            };
        }
    }
}
