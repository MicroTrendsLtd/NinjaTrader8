using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATSQuadroStrategyBase
{
    public class Common
    {
    }
}

namespace ATSQuadroStrategyBase.Helpers
{
    public class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetPropertiesAndInterfaceProperties(Type type, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            if (!type.IsInterface)
            {
                return type.GetProperties(bindingAttr);
            }

            return type.GetInterfaces().Union(new Type[] { type }).SelectMany(i => i.GetProperties(bindingAttr)).Distinct();
        }
    }
}
