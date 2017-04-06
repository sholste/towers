using SimpleMvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleMvc.Services
{
    public class Calculator : ICalculator
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
}