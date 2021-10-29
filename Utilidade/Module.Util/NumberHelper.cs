using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Util
{
    public static class NumberHelper
    {
        public static int GetRandomNumber(int max)
        {
            var rand = new Random();

            return rand.Next(0, max);
        }
    }
}
