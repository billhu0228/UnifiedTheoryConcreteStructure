using System.Collections.Generic;

namespace UnifiedTheoryConcreteStructure.Material
{
    public abstract class ConcretePropertyBase
    {

        public ConcretePropertyBase()
        {
            
        }

        public ConcretePropertyBase(string n)
        {
            _name = n;
        }

        #region 字段

        public string _name;
        #endregion

        #region 属性
        public abstract double Ec { get; }
        public abstract double Epsu { get; }
        public abstract List<string> UsedConcretPropertyList { get; }

        #endregion


        #region 方法

        public abstract double GetSigma(double eps);
        #endregion


    }
}
