using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsageObserver
{
    class MakeMeter
    {
        double Needleangle;

        public MakeMeter(double value , int x, int y,int angle, int refangle)
        {
            CalcAngle(value, angle);
        }

        private void CalcAngle(double v,int a)
        {
            Needleangle = a / 100 * v;
        }
    }
}
