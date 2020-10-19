using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 类图测试
{
    public abstract class RCItem
    {
        public string ID
        {
            get => default;
            set
            {
            }
        }

        public List<RebarTypeEnum> RebarCollection
        {
            get => default;
            set
            {
            }
        }
    }
}