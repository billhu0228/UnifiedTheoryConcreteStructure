using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 类图测试
{
    public abstract class OnePointItem : RCItem
    {
        public Point2D Location
        {
            get => default;
            set
            {
            }
        }

        public double Height
        {
            get => default;
            set
            {
            }
        }

        public Angle AngleToNormal
        {
            get => default;
            set
            {
            }
        }

        public double Pk
        {
            get => default;
            set
            {
            }
        }
    }
}